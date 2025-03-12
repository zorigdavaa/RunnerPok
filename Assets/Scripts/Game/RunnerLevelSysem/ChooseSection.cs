using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;

public class ChooseSection : LevelSection
{
    public GameObject ChooseTriggerGO;
    public override void StartSection(Level level)
    {
        base.StartSection(level);
        ChooseTriggerGO = level.lastSpawnedTile.transform.Find("Chose").gameObject;
        // float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        float ztoTest = ChooseTriggerGO.transform.position.z;
        FunctionTimer.WaitUntilAndCall(this, () => Z.Player.transform.position.z > ztoTest, () => { Skills.Instance.Show3Skills(); Skills.Instance.OnChoose += OnChooseSkill; });
    }
    public override async Task LoadNGenerateSelf()
    {
        var asyncOperation = Addressables.LoadAssetAsync<SecDataChoose>("SkillChooseSecDefault");
        await asyncOperation.Task;
        InitializefromData(asyncOperation.Result);
    }

    private void OnChooseSkill(object sender, EventArgs e)
    {
        Skills.Instance.OnChoose -= OnChooseSkill;
        curLevel.NextNearPlayer();
        EndSection();
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
