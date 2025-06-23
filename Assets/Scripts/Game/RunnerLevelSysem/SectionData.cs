using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SectionData", menuName = "ScriptableObjects/SectionData")]
public class SectionData : ScriptableObject
{
    public SectionType Type;
    public List<Tile> levelTiles;
    public Tile SectionEnd;
    public Tile SectionStart;
    public GameObject VisualPrefab;

    public virtual LevelSection CreateMono()
    {
        LevelSection section;
        GameObject newSection = new GameObject();
        switch (Type)
        {
            case SectionType.Collect:
                section = newSection.AddComponent<CollectSection>();
                // section = new CollectSection();
                break;
            default:
                section = newSection.AddComponent<LevelSection>();
                // section = new LevelSection();
                break;
        }

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;

        return section;
    }
    [ContextMenu("FillSelf")]
    public void FillSelf()
    {
        FillYourSelf();
    }
    public int SelfGenCount = 5;

    public virtual void FillYourSelf()
    {
        levelTiles.Clear();
        List<Tile> AllTiles = new List<Tile>();
        Debug.Log("base Section");
        AsyncOperationHandle loading;
        if (Type == SectionType.Collect)
        {
            loading = Addressables.LoadAssetsAsync<GameObject>("CollectTile", (obj) =>
            {
                AllTiles.Add(obj.GetComponent<Tile>());
                // Debug.Log(obj.name);
            });
        }
        else
        {
            loading = Addressables.LoadAssetsAsync<GameObject>("ObsTile", (obj) =>
            {
                AllTiles.Add(obj.GetComponent<Tile>());
                // Debug.Log(obj.name);
            });
        }

        loading.WaitForCompletion();
        // await loading.Task;
        for (int i = 0; i < SelfGenCount; i++)
        {
            levelTiles.Add(AllTiles[Random.Range(0, AllTiles.Count)]);
        }
    }
}
