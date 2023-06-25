using UnityEngine;

public class AdapticsPatternAsset : ScriptableObject
{
    [TextArea(50, 500)]
    [SerializeField]
    public string PatternJSON;
}