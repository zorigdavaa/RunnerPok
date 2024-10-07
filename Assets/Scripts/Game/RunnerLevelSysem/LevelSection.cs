using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// [CreateAssetMenu(fileName = "ObstacleSection", menuName = "ScriptableObjects/ObstacleSection")]
[System.Serializable]
public class LevelSection : Object
{
    public virtual SectionType SectionType { get => SectionType.Obstacle; }
    public EventHandler Oncomplete;
    public List<Tile> levelTiles = new List<Tile>();
    public Tile SectionEnd;
    public Tile SectionStart;
    public Level curLevel;

    public bool HasNextTile(int TileIDx)
    {
        return levelTiles.Count - 1 > TileIDx;
    }
    public virtual void StartSection(Level level)
    {
        // Debug.Log("Start of " + name);
        Init(level);
        level.player.ChangeState(PlayerState.Obs);
    }

    public void Init(Level level)
    {
        if (SectionStart)
        {
            level.SpawnTile(SectionStart);
        }
        curLevel = level;
    }

    public virtual void UpdateSection(Level level)
    {
        bool isNearEndofLand = level.player.transform.position.z > level.nextSpawnPosition.z - 70;
        if (isNearEndofLand && HasNextTile(level.SecTileIDx))
        {
            Tile tileToIns = levelTiles[level.SecTileIDx];
            level.SpawnTile(tileToIns);
            level.SecTileIDx++;
            if (!HasNextTile(level.SecTileIDx))
            {

                EndSection(level);
                // if (HasNextSection)
                // {
                //     SecIDX++;
                //     SecTileIDx = 0;
                //     StartNewSection();
                // }
                // else
                // {
                //     CurSection = null;
                //     Z.GM.LevelComplete(this, 0);
                // }
            }
        }
        // level.SpawnTile(SectionStart);
    }

    public virtual void EndSection(Level leve)
    {
        if (SectionEnd)
        {
            leve.SpawnTile(SectionEnd);
        }
        Reset();
        Oncomplete?.Invoke(this, EventArgs.Empty);
    }
    public virtual void Reset()
    {
        curLevel = null;
    }

    public List<Tile> AllTiles;
    public AssetLabelReference label;
    public virtual void LoadNGenerateSelf()
    {
        // label = AddressAbleLabelHolder.Instance.reference;
        label = new AssetLabelReference();

        SetLabel();
        // Addressables.LoadAssetsAsync<Tile>(label, (tile) =>
        // {
        //     Debug.Log("Loaded " + tile.name);
        // }).Completed += GenerateSelf;
        Addressables.LoadAssetsAsync<GameObject>(label, (tile) =>
        {
            Debug.Log("Loaded " + tile.name);
        }).Completed += GenerateSelf;
        // AllTiles = Resources.LoadAll<Tile>("Tiles").ToList();


    }

    public virtual void SetLabel()
    {
        label.labelString = "ObsTile";
    }

    public virtual void GenerateSelf(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AllTiles = handle.Result.Select(x => x.GetComponent<Tile>()).ToList();
            int SectionTileCount = 5;
            for (int j = 0; j < SectionTileCount; j++)
            {
                Tile Tile = AllTiles[Random.Range(0, AllTiles.Count)];
                levelTiles.Add(Tile);
            }
            Debug.Log("Added them ALL");
        }
        else
        {
            Debug.LogError("Failed to Load Asset");
        }
    }
}
public enum SectionType
{
    None, Obstacle, Fight, Collect
}

