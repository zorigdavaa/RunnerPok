using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Random = UnityEngine.Random;

[Serializable]
public class BaseSection
{

    public virtual SectionType SectionType { get => SectionType.Obstacle; }
    public EventHandler Oncomplete;
    public List<Tile> levelTiles;
    public List<EnemyWave> LevelEnemies;
    int index = 0;
    public Tile SectionEnd;
    public Tile SectionStart;
    public Level curLevel;
    public BaseSection()
    {
        levelTiles = new List<Tile>();
    }

    public bool HasNextTile()
    {
        return levelTiles.Count > index;
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
            Debug.Log("Start spawned " + SectionStart.name );
            level.SpawnTile(SectionStart);
        }
        curLevel = level;
    }

    public virtual void UpdateSection(Level level)
    {
        bool isNearEndofLand = level.player.transform.position.z > level.nextSpawnPosition.z - 70;
        if (isNearEndofLand && HasNextTile())
        {
            Tile tileToIns = levelTiles[index];
            level.SpawnTile(tileToIns);
            Debug.Log("at index of " + index + " tiles of " + tileToIns.name);
            index++;
            if (!HasNextTile())
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
    public string key;
    public virtual async Task LoadNGenerateSelf()
    {
        // label = AddressAbleLabelHolder.Instance.reference;

        SetKey();
        // Addressables.LoadAssetsAsync<Tile>(label, (tile) =>
        // {
        //     Debug.Log("Loaded " + tile.name);
        // }).Completed += GenerateSelf;
        AllTiles = new List<Tile>();
        levelTiles.Clear();
        var operation = Addressables.LoadAssetsAsync<GameObject>(key, (tile) =>
         {
            //  Debug.Log("Loaded " + tile.name);
             AllTiles.Add(tile.GetComponent<Tile>());
         });
        await operation.Task;
        int SectionTileCount = 3;
        for (int i = 0; i < SectionTileCount; i++)
        {
            Tile Tile = AllTiles[Random.Range(0, AllTiles.Count)];
            levelTiles.Add(Tile);
        }
    }

    public virtual void SetKey()
    {
        key = "ObsTile";
    }
}
