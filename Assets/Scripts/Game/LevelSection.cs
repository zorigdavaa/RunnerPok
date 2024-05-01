using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "ScriptableObjects/LvlSection")]
public class LevelSection : ScriptableObject
{
    public virtual SectionType SectionType { get => SectionType.Obstacle; }
    public EventHandler Oncomplete;
    public List<Tile> levelTiles;
    public Tile SectionEnd;
    public Tile SectionStart;
    public Level curLevel;

    internal bool HasNextTile(int TileIDx)
    {
        return levelTiles.Count - 1 > TileIDx;
    }
    public virtual void StartSection(Level level)
    {
        Debug.Log("Start of " + name);
        level.SpawnTile(SectionStart);
        curLevel = level;
        level.player.ChangeState(PlayerState.Obs);
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
        leve.SpawnTile(SectionEnd);
        Oncomplete?.Invoke(this, EventArgs.Empty);
        Reset();
    }
    public virtual void Reset()
    {
        curLevel = null;
    }
}
public enum SectionType
{
    None, Obstacle, Fight
}

