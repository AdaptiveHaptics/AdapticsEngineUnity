using System;
using UnityEngine;
using com.github.AdaptiveHaptics;

public class AdapticsEngineMatrix : MonoBehaviour
{
    private IntPtr engineHandle;
    void Awake()
    {
        Debug.Log(AdapticsEngineInterop.ffi_api_guard());
        engineHandle = AdapticsEngineInterop.init_adaptics_engine(false);
        Debug.Log("init adaptics engine");
    }

    void Start()
    {
        var json_pattern = Resources.Load<TextAsset>("AdapticsTestPattern").text;
        AdapticsEngineInterop.adaptics_engine_update_pattern_checked(engineHandle, json_pattern);
        Debug.Log("loaded pattern");
        double current_time_ms = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        AdapticsEngineInterop.adaptics_engine_update_playstart_checked(engineHandle, current_time_ms, 0);
        Debug.Log("updated playstart");
    }
    private void OnApplicationQuit()
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
