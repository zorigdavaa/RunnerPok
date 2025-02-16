using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ChooseSection : BaseSection
{
    public GameObject ChooseTriggerGO;
    public override void StartSection(Level level)
    {
        base.StartSection(level);
        ChooseTriggerGO = level.lastSpawnedTile.transform.Find("Chose").gameObject;
        // float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        float ztoTest = ChooseTriggerGO.transform.position.z;
        FunctionTimer.WaitUntilAndCall(this, () => Z.Player.transform.position.z > ztoTest, () => { Skills.Instance.Show3Skills(); EndSection(); });
    }
    public override void UpdateSection()
    {
        bool isNearEndofLand = curLevel.player.transform.position.z > curLevel.nextSpawnPosition.z - 50;
        if (isNearEndofLand)
        {
            // Tile tileToIns = levelTiles[Random.Range(0, levelTiles.Count)];
            Tile tileToIns = curLevel.BaseTilePf;
            curLevel.SpawnTile(tileToIns);
        }
    }
}
