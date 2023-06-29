using System;
using UnityEngine;
using com.github.AdaptiveHaptics;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class AdapticsEngineController : MonoBehaviour
{
    [Header("Adaptics Engine")]
    [Tooltip("If enabled, the Adaptics Engine will be initialized with mock streaming. This is useful for testing without a device.")]
    public bool UseMockStreaming = false;

    [Header("Visualization")]
    [Tooltip("Appropriately scaled Cube 3DObject that will be moved to visualize the playback device relative to the pattern origin.")]
    public GameObject HapticMatrixReference;
    [Tooltip("If set, the pattern will be visualized in the scene view.")]
    public LineRenderer PlaybackVisualization;

    [Header("Pattern Tracking/Translation")]
    [Tooltip("If set to an *active* GameObject, the pattern origin will be translated to the position of the tracking GameObject.")]
    public GameObject? PatternTrackingObject;
    [Tooltip("If enabled, the vertical coordinate of 'Pattern Z' (corresponding to Unity's Y-axis) will be overridden when PatternTrackingObject is active.")]  
    public bool IgnorePlaybackHeightWhenTracking = true;

    private ulong engineHandle;
    void Awake()
    {
        Debug.Log("initializing adaptics engine '" + AdapticsEngineInterop.ffi_api_guard() + "' with use_mock_streaming: " + UseMockStreaming);
        engineHandle = AdapticsEngineInterop.init_adaptics_engine(UseMockStreaming, true);
        //Debug.Log("init adaptics engine");
    }
    private void Start()
    {
        if (HapticMatrixReference == null) Debug.LogError("HapticMatrixReference must be set");
        if (HapticMatrixReference.transform.parent != transform) Debug.LogError("HapticMatrixReference must be a child of AdapticsEngineController");
        // The local origin (0,0,0) inside this game object (AdapticsEngineController) is the same as the pattern origin
        // So we must move the reference device down by half its height so it visually aligns with the pattern origin
        HapticMatrixReference.transform.localPosition = new Vector3(0, - HapticMatrixReference.transform.localScale.y / 2, 0);
    }

    private double LastEvalUpdatePatternTime;
    private bool wasTracking = false;
    private void Update()
    {
        if (PlaybackVisualization)
        {
            UnityEvalResult[] playback_updates = new UnityEvalResult[1024]; //660-740 for ~30hz updates at 20000hz device rate
            AdapticsEngineInterop.adaptics_engine_get_playback_updates(engineHandle, playback_updates, out uint num_evals);
            //ColorUtility.TryParseHtmlString("#74d4ec", out var color_pattern_playback_vis_high);
            ColorUtility.TryParseHtmlString("#2DA8D6", out var color_pattern_playback_vis_high); //adjusted for hdr/multiply?
            ColorUtility.TryParseHtmlString("#263d4e", out var color_pattern_playback_vis_low); //same as html, adjusted for hdr/multiply is too dark
            if (num_evals > 0)
            {
                //Debug.Log("got " + num_evals + " playback updates");
                PlaybackVisualization.positionCount = (int)num_evals;
                var sum_alpha = 0.0;
                for (int i = 0; i < num_evals; i++)
                {
                    var eval = playback_updates[i];
                    PlaybackVisualization.SetPosition(i, new Vector3((float)eval.coords.x, (float)eval.coords.y, (float)eval.coords.z));
                    sum_alpha += eval.intensity;
                }
                LastEvalUpdatePatternTime = playback_updates[num_evals - 1].pattern_time;
                var alpha = (float)sum_alpha / num_evals;
                var color = Color.Lerp(color_pattern_playback_vis_low, color_pattern_playback_vis_high, alpha);
                PlaybackVisualization.material.color = color;
            }
        }
        if (PatternTrackingObject && PatternTrackingObject.activeInHierarchy)
        {
            wasTracking = true;
            var delta = PatternTrackingObject.transform.position - transform.position;
            Matrix4x4 matrix = Matrix4x4.Translate(delta);
            if (IgnorePlaybackHeightWhenTracking)
            {
                matrix = matrix * Matrix4x4.Scale(new Vector3(1, 0, 1)); // ignore vertical (pattern z, unity y) when tracking
            }
            UpdateGeometricTransformMatrix(matrix);
        } else if (wasTracking)
        {
            wasTracking = false;
            UpdateGeometricTransformMatrix(Matrix4x4.identity);
            Debug.Log(Matrix4x4.identity);
        }
    }


    private string SerializeUserParameters()
    {
        string serialized = "{";
        for ( int i = 0; i < UserParameters.Count; i++)
        {
            var kvp = UserParameters.ElementAt(i);
            serialized += "\"" + kvp.Key + "\": " + kvp.Value;
            if (i < UserParameters.Count - 1)
            {
                serialized += ",\n";
            }
        }
        serialized += "}";
        return serialized;
    }

    private Dictionary<string, double> UserParameters = new Dictionary<string, double>();
    public void UpdateUserParameter(string name, double value)
    {
        UserParameters[name] = value;
        var user_params_json = SerializeUserParameters();
        AdapticsEngineInterop.adaptics_engine_update_user_parameters_checked(engineHandle, user_params_json);
    }

    public void UpdateGeometricTransformMatrix(Matrix4x4 matrixInMeters)
    {
        Matrix4x4 yz_element_swap = Matrix4x4.identity;
        yz_element_swap[1,1] = 0; yz_element_swap[1,2] = 1;
        yz_element_swap[2,1] = 1; yz_element_swap[2,2] = 0;
        Matrix4x4 mm_to_meters = Matrix4x4.Scale(new Vector3(0.001f, 0.001f, 0.001f));
        Matrix4x4 meters_to_mm = Matrix4x4.Scale(new Vector3(1000, 1000, 1000));

        // This is ok because FullSimplify[su.perspectivedivide[m.sd.p] == perspectivedivide[su.m.sd.p]] is true.
        // Where:
        //   `sd` is from mm to m
        //   `su` is from m to mm
        //   `p` is a point in mm {x,y,z,1}
        //   `m` is any 4x4 matrix where the bottom right element m[3,3] == 1
        var matrix = yz_element_swap * meters_to_mm * matrixInMeters * mm_to_meters * yz_element_swap;
        //Debug.Log("***************");
        //Debug.Log(matrixInMeters);
        //Debug.Log(matrix);

        GeoMatrix m = new GeoMatrix();
        m.data0 = matrix[0,0]; m.data1 = matrix[0,1]; m.data2 = matrix[0,2]; m.data3 = matrix[0,3];
        m.data4 = matrix[1,0]; m.data5 = matrix[1,1]; m.data6 = matrix[1,2]; m.data7 = matrix[1,3];
        m.data8 = matrix[2,0]; m.data9 = matrix[2,1]; m.data10 = matrix[2,2]; m.data11 = matrix[2,3];
        m.data12 = matrix[3,0]; m.data13 = matrix[3,1]; m.data14 = matrix[3,2]; m.data15 = matrix[3,3];
        AdapticsEngineInterop.adaptics_engine_update_geo_transform_matrix_checked(engineHandle, m);
    }

    public void ResetEvalParameters()
    {
        UserParameters.Clear();
        AdapticsEngineInterop.adaptics_engine_reset_parameters_checked(engineHandle);
    }

    private static double GetCurrentTimeMs() { return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds; }
    private Hash128 LastPlayedPatternHash;
    public void PlayPattern(AdapticsPatternAsset pattern)
    {
        AdapticsEngineInterop.adaptics_engine_update_pattern_checked(engineHandle, pattern.PatternJson);
        LastPlayedPatternHash = pattern.PatternJsonHash;
        ResetEvalParameters();
        Debug.Log("loaded pattern '" + pattern.name + "'");
        double current_time_ms = GetCurrentTimeMs();
        AdapticsEngineInterop.adaptics_engine_update_playstart_checked(engineHandle, current_time_ms, 0);
        //Debug.Log("started pattern");
    }
    public void PausePlayback()
    {
        AdapticsEngineInterop.adaptics_engine_update_playstart_checked(engineHandle, 0, 0);
        //Debug.Log("paused pattern");
    }
    /// <summary>
    /// Resume time is based on the last value returned from adaptics_engine_get_playback_updates, so the resume time point may be slightly before the exact pause time point. With no lag this will be less than 1/30th of a second.
    /// </summary>
    /// <returns>
    /// If there is no pattern currently playing and nothing was resumed, false. Otherwise, true.
    /// </returns>
    public bool ResumePlayback()
    {
        if (LastPlayedPatternHash != null)
        {
            var playstart = GetCurrentTimeMs() - LastEvalUpdatePatternTime;
            AdapticsEngineInterop.adaptics_engine_update_time(engineHandle, LastEvalUpdatePatternTime);
            AdapticsEngineInterop.adaptics_engine_update_playstart_checked(engineHandle, playstart, -LastEvalUpdatePatternTime);
            //Debug.Log("resumed pattern");
            return true;
        } else
        {
            return false;
        }
    }
    public void ResumeOrPlayPattern(AdapticsPatternAsset pattern)
    {
        if (LastPlayedPatternHash == pattern.PatternJsonHash)
        {
            ResumePlayback();
        } else
        {
            PlayPattern(pattern);
        }
    }
    public void StopPlayback()
    {
        AdapticsEngineInterop.adaptics_engine_update_playstart_checked(engineHandle, 0, 0);
        Debug.Log("stopped pattern");
    }


    private void OnDestroy()
    {
        Debug.Log("pre deinit_adaptics_engine");
        byte[] err_msg = new byte[1024];
        try
        {
            AdapticsEngineInterop.deinit_adaptics_engine(engineHandle, err_msg);
        } catch (InteropException<FFIError> e)
        {
            if (e.Error == FFIError.ErrMsgProvided)
            {
                var err_msg_str = System.Text.Encoding.UTF8.GetString(err_msg).TrimEnd('\0');
                throw new InvalidOperationException("deinit_adaptics_engine error: " + err_msg_str);
            }
            throw e;
        }
        Debug.Log("deinit_adaptics_engine");
    }

}
