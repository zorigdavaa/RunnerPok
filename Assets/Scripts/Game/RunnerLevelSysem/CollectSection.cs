using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [CreateAssetMenu(fileName = "CollectSection", menuName = "ScriptableObjects/CollectSection")]
public class CollectSection : LevelSection
{
    public override SectionType SectionType => SectionType.Collect;
    public override void StartSection(Level level)
    {
        Init(level);
        Tile tileToIns = levelTiles[0];
        // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        Tile tile = level.SpawnTile(tileToIns);
        level.lastSpawnedTile.OnTileEnter += OnCollectSectionEnter;
        // Debug.Log("Start Of From Section " + SectionType + " " + name);
        // Tile tileToIns = levelTiles[0];
        // // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        // Tile tile = level.SpawnTile(tileToIns);
        // tile.OnTileEnter += OnCollectSectionEnter;
    }

    public override void UpdateSection(Level level)
    {
        base.UpdateSection(level);
    }
    public override void EndSection(Level leve)
    {
        base.EndSection(leve);
        // Debug.Log("End of Collect " + name);
    }
    private void OnCollectSectionEnter(object sender, EventArgs e)
    {
        Tile casted = (Tile)sender;
        casted.OnTileEnter -= OnCollectSectionEnter;
        // Debug.Log("Entering " + name);
        if (curLevel)
        {
            curLevel.player.ChangeState(PlayerState.Collect);
        }
    }
    public override void SetKey()
    {
        key = "CollectTile";
    }
}
