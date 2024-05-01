using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossSection : FightSection
{
    public Enemy Boss;
    public override void StartSection(Level level)
    {
        Reset();
        Init(level);
        Tile tileToIns = levelTiles[0];
        Tile tile = level.SpawnTile(tileToIns);
        tile.OnTileEnter += OnFightSectionEnter;
        InsSoldiers(tile);
    }

    private void OnFightSectionEnter(object sender, EventArgs e)
    {
        curLevel.player.ChangeState(PlayerState.Fight);
    }
    public void InsSoldiers(Tile tile)
    {
        // player.GoingToFight(true);

        curLevel.StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            EnemyWave Wave = LevelEnemies[EnemyWaveIdx];
            List<Enemy> InsEnems = Wave.EnemyPF;
            if (Wave.Beforedelay > 0)
            {
                yield return new WaitForSeconds(Wave.Beforedelay);
            }
            for (int i = 0; i < InsEnems.Count; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-4, 4), 0, 0);
                Enemy insEnemy = Instantiate(InsEnems[i], pos, Quaternion.Euler(0, 180, 0), tile.transform);
                insEnemy.Ondeath += OnEnemyDeath;
                RemainingEnemy++;
            }
            if (HasNextWave(EnemyWaveIdx))
            {
                EnemyWave NextWave = LevelEnemies[EnemyWaveIdx + 1];
                if (NextWave.Wait)
                {
                    yield return new WaitForSeconds(NextWave.AfterDelay);
                    Debug.Log("Next Wave");
                    EnemyWaveIdx++;
                    StartInstantiateEnemies();
                }
            }
            yield return null;
        }
    }
}
