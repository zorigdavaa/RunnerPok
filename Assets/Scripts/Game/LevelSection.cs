using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSection", menuName = "ScriptableObjects/LvlSection")]
public class LevelSection : ScriptableObject
{
    public SectionType SectionType;
    public EventHandler Oncomplete;
    public List<Tile> levelTiles;
    public Tile SectionEnd;
    public Tile SectionStart;
    public List<EnemyWave> LevelEnemies;
    internal bool HasNextWave(int secTileIDx)
    {
        return levelTiles.Count - 1 > secTileIDx;
    }
    internal bool HasNextTile(int TileIDx)
    {
        return levelTiles.Count - 1 > TileIDx;
    }
    internal virtual void StartSection(Level startingLevel)
    {
        startingLevel.SpawnTile(SectionStart);
    }

    internal virtual void EndSection(Level endingLevel)
    {
        endingLevel.SpawnTile(SectionEnd);
    }
}
public enum SectionType
{
    None, Obstacle, Fight
}

