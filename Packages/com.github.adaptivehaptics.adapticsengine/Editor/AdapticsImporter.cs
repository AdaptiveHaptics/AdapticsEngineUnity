using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

[ScriptedImporter(1, "adaptics")]
public class AdapticsImporter : ScriptedImporter
{
    public override void OnImportAsset(AssetImportContext ctx)
    {
        var jsontext = File.ReadAllText(ctx.assetPath);
        var patternAsset = ScriptableObject.CreateInstance<AdapticsPatternAsset>();
        patternAsset.PatternJson = jsontext;
        patternAsset.PatternJsonHash = Hash128.Compute(jsontext);
        ctx.AddObjectToAsset("Adaptics Pattern Asset", patternAsset);
        ctx.SetMainObject(patternAsset);
    }
}
