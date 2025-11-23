using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ComplexSection", menuName = "ScriptableObjects/ComplexSection")]
public class SectionCData : SectionData
{
    public List<GameObject> Obstacles;
    public GameObject UpHillObs;
    public GameObject Coin;
    public List<GameObject> Boosters;

    public override LevelSection CreateMono()
    {
        GameObject newOBj = new GameObject();
        // ObsSection section = new ObsSection();
        CSection section = newOBj.AddComponent<CSection>();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.Obstacles = Obstacles;

        section.Coin = Coin;
        section.Boosters = Boosters;
        section.UpHillObs = UpHillObs;
        return section;
    }
    public override void FillYourSelf()
    {
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
        paths = AddressableHelper.GetPrefabPathssByLabel("Coin");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Coin = prefab;
            break;
        }
        paths = AddressableHelper.GetPrefabPathssByLabel("UpHill");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            UpHillObs = prefab;
            break;
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
    }
}
