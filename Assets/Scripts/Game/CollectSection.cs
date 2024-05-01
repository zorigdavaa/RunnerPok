using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CollectSection", menuName = "ScriptableObjects/Collect")]
public class CollectSection : LevelSection
{
    public override SectionType SectionType => SectionType.Collect;
    public override void StartSection(Level level)
    {
        curLevel = level;
        Tile tileToIns = levelTiles[0];
        // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        Tile tile = level.SpawnTile(tileToIns);
        tile.OnTileEnter += OnCollectSectionEnter;
    }

    public override void UpdateSection(Level level)
    {
        base.UpdateSection(level);
    }
    public override void EndSection(Level leve)
    {
        base.EndSection(leve);
    }
    private void OnCollectSectionEnter(object sender, EventArgs e)
    {
        Tile casted = (Tile)sender;
        casted.OnTileEnter -= OnCollectSectionEnter;
        curLevel.player.ChangeState(PlayerState.Collect);
    }
}
