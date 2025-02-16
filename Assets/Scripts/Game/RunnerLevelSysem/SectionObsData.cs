using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "ObsSection", menuName = "ScriptableObjects/ObsSection")]
public class SectionObsData : SectionData
{
    public List<GameObject> Obstacles;
    public override BaseSection CreateMono()
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
        levelTiles.Clear();
        Obstacles.Clear();
        List<GameObject> AllObs = new List<GameObject>();
        AsyncOperationHandle loading;
        loading = Addressables.LoadAssetsAsync<GameObject>("default", (obj) =>
        {
            levelTiles.Add(obj.GetComponent<Tile>());
        });
        loading.WaitForCompletion();
        loading = Addressables.LoadAssetsAsync<GameObject>("Obstacle", (obj) =>
        {
            AllObs.Add(obj);
        });
        loading.WaitForCompletion();
        for (int i = 0; i < SelfGenCount; i++)
        {
            Obstacles.Add(AllObs[Random.Range(0, AllObs.Count)]);
        }
    }
}
