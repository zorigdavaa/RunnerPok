using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

// [CreateAssetMenu(fileName = "BossSection", menuName = "ScriptableObjects/BossSection")]
[Serializable]
public class BossSection : FightSection
{
    [SerializeReference] public EnemyWave Boss;
    public override void StartSection(Level level)
    {
        Reset();
        Init(level);
        Tile tileToIns = levelTiles[0];
        Tile tile = level.SpawnTile(tileToIns);
        tile.OnTileEnter += OnFightSectionEnter;
        InsEnemsAtTile(tile);
    }

    private void OnFightSectionEnter(object sender, EventArgs e)
    {
        curLevel.player.ChangeState(PlayerState.Fight);
        InsBoss();
        Transform parnet = curLevel.player.GetComponent<PlayerMovement>().playerParent;
        foreach (var item in InsEnemies)
        {
            item.transform.SetParent(parnet);
        }
    }
    List<Enemy> InsEnemies = new List<Enemy>();
    public void InsEnemsAtTile(Tile tile)
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
                Vector3 tileCenter = (tile.start.position + tile.end.position) / 2;
                Vector3 pos = tileCenter + new Vector3(Random.Range(-4, 4), 0, 0);
                Enemy insEnemy = GameObject.Instantiate(InsEnems[i], pos, Quaternion.Euler(0, 180, 0), tile.transform);
                // insEnemy.Ondeath += OnEnemyDeath;
                // RemainingEnemy++;
                InsEnemies.Add(insEnemy);
            }
            if (HasNextWave(EnemyWaveIdx))
            {
                EnemyWave NextWave = LevelEnemies[EnemyWaveIdx + 1];
                if (NextWave.Wait)
                {
                    yield return new WaitForSeconds(NextWave.AfterDelay);
                    Debug.Log("Next Wave");
                    EnemyWaveIdx++;
                    InsEnemsAtTile(tile);
                }
            }
            yield return null;
        }
    }
    public void InsBoss()
    {
        // player.GoingToFight(true);
        curLevel.player.ChangeState(PlayerState.Fight);
        curLevel.StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            Transform playerParent = curLevel.player.GetComponent<PlayerMovement>().playerParent;
            if (Boss.Beforedelay > 0)
            {
                yield return new WaitForSeconds(Boss.Beforedelay);
            }
            Debug.Log(EnemyWaveIdx + " new Wave");
            for (int i = 0; i < Boss.EnemyPF.Count; i++)
            {
                Vector3 RandomPosXZ = new Vector3(Random.Range(-4, 4), 0, 0);
                Enemy insEnemy = GameObject.Instantiate(Boss.EnemyPF[i], playerParent.position + Vector3.forward * 20 + RandomPosXZ, Quaternion.Euler(0, 180, 0), playerParent);
                insEnemy.Ondeath += OnEnemyDeath;
                AllEnemyCount++;
            }
            if (HasNextWave(EnemyWaveIdx))
            {
                EnemyWave NextWave = LevelEnemies[EnemyWaveIdx + 1];
                if (NextWave.Wait)
                {
                    yield return new WaitForSeconds(NextWave.AfterDelay);
                    Debug.Log("Next Wave");
                    EnemyWaveIdx++;
                    InsEnemsBeforePlayer();
                }
            }
            yield return null;
        }
    }
    public override void Reset()
    {
        base.Reset();
        InsEnemies = new List<Enemy>();
    }
    //When boss death killing ins enemies
    public override void OnEnemyDeath(object sender, EventArgs e)
    {
        foreach (var item in InsEnemies)
        {
            item.Health = 1;
            item.TakeDamage(-1);
        }
        EndSection();
    }
    public override void SetKey()
    {
        key = "FightTile";
    }
}
