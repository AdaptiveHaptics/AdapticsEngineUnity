using System;
using UnityEngine;
using com.github.AdaptiveHaptics;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

public class AdapticsEngineController : MonoBehaviour
{
    public bool UseMockStreaming = false;

    public LineRenderer PlaybackVisualization;

    private ulong engineHandle;
    void Awake()
    {
        Debug.Log("initializing adaptics engine '" + AdapticsEngineInterop.ffi_api_guard() + "' with use_mock_streaming: " + UseMockStreaming);
        engineHandle = AdapticsEngineInterop.init_adaptics_engine(UseMockStreaming, true);
        //Debug.Log("init adaptics engine");
    }

    private double LastEvalUpdatePatternTime;
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


    private static double GetCurrentTimeMs() { return (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds; }
    private Hash128 LastPlayedPatternHash;
    public void PlayPattern(AdapticsPatternAsset pattern)
    {
        AdapticsEngineInterop.adaptics_engine_update_pattern_checked(engineHandle, pattern.PatternJson);
        LastPlayedPatternHash = pattern.PatternJsonHash;
        UserParameters.Clear(); AdapticsEngineInterop.adaptics_engine_reset_parameters_checked(engineHandle);
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
