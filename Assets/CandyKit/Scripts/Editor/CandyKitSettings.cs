using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using System.IO;
using System.Collections.Generic;

namespace CandyKitSDK
{
    [CanEditMultipleObjects]
    public class CandyKitSettings : EditorWindow
    {
        public const string path = "Assets/Resources/CandyKit/CandyKitSettings.asset";

        [MenuItem("CandyKit/Settings")]
        private static void ShowSettings()
        {
            var asset = AssetDatabase.LoadMainAssetAtPath(path);

            if (asset)
            {
                AssetDatabase.OpenAsset(asset);
            }
            else
            {
                if (!Directory.Exists("Assets/Resources"))
                {
                    Directory.CreateDirectory("Assets/Resources");
                }
                Directory.CreateDirectory("Assets/Resources/CandyKit");
                CandyKitSettingsScriptableObject settingsAsset = ScriptableObject.CreateInstance<CandyKitSettingsScriptableObject>();

                AssetDatabase.CreateAsset(settingsAsset, "Assets/Resources/CandyKit/CandyKitSettings.asset");
                AssetDatabase.SaveAssets();

                AssetDatabase.OpenAsset(settingsAsset);
            }
        }
    }
}
