#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets;
using UnityEngine;
using System.Collections.Generic;

public static class AddressableHelper
{
    public static List<string> GetPrefabPathssByLabel(string label)
    {
        List<string> guids = new List<string>();
        var settings = AddressableAssetSettingsDefaultObject.Settings;
        if (settings == null) return guids;

        foreach (var group in settings.groups)
        {
            foreach (var entry in group.entries)
            {
                if (entry.labels.Contains(label))
                {
                    string path = entry.AssetPath; // Project path
                    // string guid = AssetDatabase.AssetPathToGUID(path);
                    guids.Add(path);
                    Debug.Log($"Prefab {entry.address} path: {path}");
                }
            }
        }
        return guids;
    }
}
#endif
