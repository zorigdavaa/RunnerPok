using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using ZPackage;

// [CreateAssetMenu(fileName = "CollectSection", menuName = "ScriptableObjects/CollectSection")]
public class CollectSection : LevelSection
{
    public override SectionType SectionType => SectionType.Collect;
    public override void StartSection(Level level)
    {
        Init(level);
        EnterThisSection();
        // Tile tileToIns = levelTiles[0];
        // // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        // Tile tile = level.SpawnTile(tileToIns);
        // level.lastSpawnedTile.OnTileEnter += OnCollectSectionEnter;

    }
    private void EnterThisSection()
    {
        float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        FunctionTimer.WaitUntilAndCall(curLevel, () => Z.Player.transform.position.z > ztoTest, () => { OnCollectSectionEnter(this, EventArgs.Empty); });
    }

    public override void UpdateSection()
    {
        base.UpdateSection();
    }
    public override void EndSection()
    {
        base.EndSection();
        // Debug.Log("End of Collect " + name);
    }
    public override Task LoadNGenerateSelf()
    {
        GenerateTileCount = 1;
        return base.LoadNGenerateSelf();
    }
    private void OnCollectSectionEnter(object sender, EventArgs e)
    {
        // Tile casted = (Tile)sender;
        // casted.OnTileEnter -= OnCollectSectionEnter;
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
    // public override Task LoadNGenerateSelf()
    // {
    //     return base.LoadNGenerateSelf();
    // }
}
