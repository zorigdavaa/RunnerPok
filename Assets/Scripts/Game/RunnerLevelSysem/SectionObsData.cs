using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "ObsSection", menuName = "ScriptableObjects/ObsSection")]
public class SectionObsData : SectionData
{
    public List<GameObject> Obstacles;
    public override LevelSection CreateMono()
    {
        GameObject newOBj = new GameObject();
        // ObsSection section = new ObsSection();
        ObsSection section = newOBj.AddComponent<ObsSection>();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.Obstacles = Obstacles;
        return section;
    }
    public override void FillYourSelf()
    {
#if UNITY_EDITOR
        levelTiles = new List<Tile>();
        Obstacles = new List<GameObject>();
        List<GameObject> AllObs = new List<GameObject>();

        var paths = AddressableHelper.GetPrefabPathssByLabel("default");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            levelTiles.Add(prefab.GetComponent<Tile>());
        }
        paths = AddressableHelper.GetPrefabPathssByLabel("Obstacle");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            AllObs.Add(prefab);
        }
        // AsyncOperationHandle loading;
        // loading = Addressables.LoadAssetsAsync<GameObject>("default", (obj) =>
        // {
        //     levelTiles.Add(obj.GetComponent<Tile>());
        // });
        // loading.WaitForCompletion();
        // loading = Addressables.LoadAssetsAsync<GameObject>("Obstacle", (obj) =>
        // {
        //     AllObs.Add(obj);
        // });
        // loading.WaitForCompletion();
        for (int i = 0; i < SelfGenCount; i++)
        {
            Obstacles.Add(AllObs[Random.Range(0, AllObs.Count)]);
        }
        SaveChanges();
#endif
    }
}
