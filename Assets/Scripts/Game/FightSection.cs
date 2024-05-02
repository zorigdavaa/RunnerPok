using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "FightSection", menuName = "ScriptableObjects/FightSection")]
public class FightSection : LevelSection
{
    public override SectionType SectionType => SectionType.Fight;
    public List<EnemyWave> LevelEnemies;
    [NonSerialized]
    public int EnemyWaveIdx = 0;
    [NonSerialized]
    public int AllEnemyCount = 0;
    [NonSerialized]
    public int RemEnemyCount = 0;

    internal bool HasNextWave(int secTileIDx)
    {
        return LevelEnemies.Count - 1 > secTileIDx;
    }
    public override void StartSection(Level level)
    {
        Reset();
        base.StartSection(level);
        Tile tileToIns = levelTiles[0];
        // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        Tile tile = level.SpawnTile(tileToIns);
        tile.OnTileEnter += OnFightSectionEnter;
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
        Debug.Log("End Fight " + EnemyWaveIdx);
        base.EndSection(leve);
        leve.player.ChangeState(PlayerState.Obs);
    }
    public override void Reset()
    {
        // base.Reset();
        EnemyWaveIdx = 0;
        AllEnemyCount = 0;
        RemEnemyCount = 0;
    }

    void OnFightSectionEnter(object sender, EventArgs e)
    {
        Tile casted = (Tile)sender;
        casted.OnTileEnter -= OnFightSectionEnter;
        // print("Start Insing");
        InsEnemsBeforePlayer();
        AllEnemyCount = LevelEnemies.Sum(x => x.EnemyPF.Count);
        Debug.Log("Rem enem " + AllEnemyCount);
    }

    public virtual void InsEnemsBeforePlayer()
    {
        // player.GoingToFight(true);
        curLevel.player.ChangeState(PlayerState.Fight);
        curLevel.StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            EnemyWave Wave = LevelEnemies[EnemyWaveIdx];
            List<Enemy> InsEnems = Wave.EnemyPF;
            Transform playerParent = curLevel.player.GetComponent<PlayerMovement>().playerParent;
            if (Wave.Beforedelay > 0)
            {
                yield return new WaitForSeconds(Wave.Beforedelay);
            }
            Debug.Log(EnemyWaveIdx + " new Wave");
            for (int i = 0; i < InsEnems.Count; i++)
            {
                Vector3 RandomPosXZ = new Vector3(Random.Range(-4, 4), 0, 0);
                Enemy insEnemy = Instantiate(InsEnems[i], playerParent.position + Vector3.forward * 20 + RandomPosXZ, Quaternion.Euler(0, 180, 0), playerParent);
                insEnemy.Ondeath += OnEnemyDeath;
                RemEnemyCount++;
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


    public virtual void OnEnemyDeath(object sender, EventArgs e)
    {

        RemEnemyCount--;
        Debug.Log("deatj " + AllEnemyCount);
        if (RemEnemyCount == 0)
        {
            if (HasNextWave(EnemyWaveIdx))
            {
                Debug.Log("Next Wave");
                EnemyWaveIdx++;
                InsEnemsBeforePlayer();
            }
            else
            {

                EndSection(curLevel);
            }
        }
    }
}
