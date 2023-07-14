using UnityEngine;

public class AdapticsPatternAsset : ScriptableObject
{
    public Hash128 HashOfPatternJson;

    [TextArea(50, 500)]
    [SerializeField]
    public string PatternJson;
}