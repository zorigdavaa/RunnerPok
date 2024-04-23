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
    public List<EnemyWave> LevelEnemies;
    public Tile SectionEnd;
    public Tile SectionStart;

    internal bool HasNextTile(int TileIDx)
    {
        return levelTiles.Count - 1 > TileIDx;
    }

    internal bool HasNextWave(int secTileIDx)
    {
        throw new NotImplementedException();
    }

    internal void StartSection(Level startingLevel)
    {
        startingLevel.SpawnTile(SectionStart);
    }
    internal void EndSection(Level endingLevel)
    {
        endingLevel.SpawnTile(SectionEnd);
    }
}
public enum SectionType
{
    None, Obstacle, Fight
}

