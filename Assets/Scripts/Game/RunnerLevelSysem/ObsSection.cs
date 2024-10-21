using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;
using Random = UnityEngine.Random;

public class ObsSection : BaseSection
{
    // implement this section this is runtime baseTile spawner and instantiate obstacle based on situation
    public override void StartSection(Level level)
    {
        Reset();
        base.StartSection(level);
        // Tile tileToIns = levelTiles[0];
        // // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        // Tile tile = level.SpawnTile(tileToIns);
        // tile.OnTileEnter += OnFightSectionEnter;
        EnterThisSection();
    }
    private void EnterThisSection()
    {
        Debug.Log("eNTER FIREsECTION");
        float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        FunctionTimer.WaitUntilAndCall(curLevel, () => Z.Player.transform.position.z > ztoTest, () => { OnObsSectionEnter(this, EventArgs.Empty); });
    }
    public override void UpdateSection(Level level)
    {
        bool isNearEndofLand = level.player.transform.position.z > level.nextSpawnPosition.z - 50;
        if (isNearEndofLand)
        {
            Tile tileToIns = levelTiles[Random.Range(0, levelTiles.Count)];
            level.SpawnTile(tileToIns);
        }
    }
    public override void EndSection(Level leve)
    {
        base.EndSection(leve);
        leve.player.ChangeState(PlayerState.Obs);
    }
    void OnObsSectionEnter(object sender, EventArgs e)
    {
        InsObsBeforePlayer();
    }
    public virtual void InsObsBeforePlayer()
    {
        // curLevel.StartCoroutine(LocalCoroutine());
        // IEnumerator LocalCoroutine()
        // {
        //     EnemyWave Wave = LevelEnemies[EnemyWaveIdx];
        //     List<Enemy> InsEnems = Wave.EnemyPF;
        //     Transform playerParent = curLevel.player.GetComponent<PlayerMovement>().playerParent;
        //     if (Wave.Beforedelay > 0)
        //     {
        //         yield return new WaitForSeconds(Wave.Beforedelay);
        //     }
        //     Debug.Log(EnemyWaveIdx + " new Wave");
        //     for (int i = 0; i < InsEnems.Count; i++)
        //     {
        //         Vector3 RandomPosXZ = new Vector3(Random.Range(-4, 4), 0, 0);
        //         Enemy insEnemy = GameObject.Instantiate(InsEnems[i], playerParent.position + Vector3.forward * 20 + RandomPosXZ, Quaternion.Euler(0, 180, 0), playerParent);
        //         insEnemy.Ondeath += OnEnemyDeath;
        //         RemEnemyCount++;
        //     }
        //     if (HasNextWave(EnemyWaveIdx))
        //     {
        //         EnemyWave NextWave = LevelEnemies[EnemyWaveIdx + 1];
        //         if (NextWave.Wait)
        //         {
        //             yield return new WaitForSeconds(NextWave.AfterDelay);
        //             Debug.Log("Next Wave");
        //             EnemyWaveIdx++;
        //             InsObsBeforePlayer();
        //         }
        //     }
        //     yield return null;
        // }
    }
}
