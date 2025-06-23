using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SectionData))]
public class SectionDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SectionData sectionData = (SectionData)target;

        if (GUILayout.Button("Fill Self"))
        {
            sectionData.FillYourSelf();
            EditorUtility.SetDirty(sectionData);
        }

        if (GUILayout.Button("Create Visual"))
        {
            if (sectionData.VisualPrefab)
            {
                Debug.LogWarning("Visual prefab already exists. Please delete it first if you want to recreate it.");
                return;
            }

            GameObject parentGameObject = new GameObject(sectionData.name);
            Tile parentTile = parentGameObject.AddComponent<Tile>();

            Vector3 nextPosition = Vector3.zero;
            for (int i = 0; i < sectionData.levelTiles.Count; i++)
            {
                Tile tile = sectionData.levelTiles[i];
                if (tile == null)
                {
                    Debug.LogWarning($"Tile at index {i} is null. Skipping.");
                    continue;
                }

                if (i == 0)
                    parentTile.start = tile.start;
                else if (i == sectionData.levelTiles.Count - 1)
                    parentTile.end = tile.end;

                // Object prefabSource = PrefabUtility.GetCorrespondingObjectFromSource(tile);
                Tile tileInstance = (Tile)PrefabUtility.InstantiatePrefab(tile, parentGameObject.transform);
                tileInstance.transform.position = nextPosition;
                Tile tileObj = tileInstance.GetComponent<Tile>();
                nextPosition = tileObj.end.position;
            }

#if UNITY_EDITOR
            // Save prefab to the same folder as the SectionData asset
            string assetPath = AssetDatabase.GetAssetPath(sectionData);
            string folderPath = System.IO.Path.GetDirectoryName(assetPath);
            string prefabPath = $"{folderPath}/{sectionData.name}_Visual.prefab";
            Debug.Log($"Saving prefab to: {prefabPath}");

            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(parentGameObject, prefabPath);
            sectionData.VisualPrefab = prefab.GetComponent<Tile>();

            AssetDatabase.SaveAssets();
            GameObject.DestroyImmediate(parentGameObject); // clean up scene
#endif
        }
    }
}
