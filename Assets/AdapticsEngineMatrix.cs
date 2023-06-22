using System;
using UnityEngine;
using com.github.AdaptiveHaptics;

public class AdapticsEngineMatrix : MonoBehaviour
{
    private IntPtr engineHandle;
    void Awake()
    {
        print(AdapticsEngineInterop.ffi_api_guard());
        engineHandle = AdapticsEngineInterop.init_adaptics_engine(false);
        print("init adaptics engine");
    }

    void Start()
    {
        var json_pattern = Resources.Load<TextAsset>("AdapticsTestPattern").text;
        AdapticsEngineInterop.adaptics_engine_update_pattern(engineHandle, json_pattern);
        print("loaded pattern");
        double current_time_ms = (DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds;
        AdapticsEngineInterop.adaptics_engine_update_playstart(engineHandle, current_time_ms, 0);
        print("updated playstart");
    }
    private void OnApplicationQuit()
    {
        print("pre deinit adaptics engine");
        AdapticsEngineInterop.deinit_adaptics_engine(engineHandle);
        print("deinit adaptics engine");
    }
}
