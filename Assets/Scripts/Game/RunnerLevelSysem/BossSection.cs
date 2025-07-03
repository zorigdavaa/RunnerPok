using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;
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
        if (levelTiles == null || !levelTiles.Any())
        {
            levelTiles = new List<Tile>();
            GameObject loadTile = Addressables.LoadAssetAsync<GameObject>("default").WaitForCompletion();
            levelTiles.Add(loadTile.GetComponent<Tile>());
            Debug.Log("Loaded");
        }
        Tile tileToIns = levelTiles[0];
        Tile tile = level.SpawnTile(tileToIns);
        // tile.OnTileEnter += OnFightSectionEnter;
        FunctionTimer.WaitUntilAndCall(this, () => Z.Player.transform.position.z > tile.start.transform.position.z, () => { OnFightSectionEnter(this, EventArgs.Empty); });
        if (InsEnemies.Any())
        {
            InsEnemsAtTile(tile);
        }
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
    public override async Task LoadNGenerateSelf()
    {
        Boss = new EnemyWave();
        List<GameObject> AllBoss = new List<GameObject>();
        var asyncOperation = Addressables.LoadAssetsAsync<GameObject>("Boss", (boss) =>
        {
            AllBoss.Add(boss);
        });
        await asyncOperation.Task;
        GameObject chosenBoss = AllBoss[Random.Range(0, AllBoss.Count)];
        Boss.EnemyPF.Add(chosenBoss.GetComponent<Animal>());
        await base.LoadNGenerateSelf();
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
            float needDamdage = item.Stats.Health.GetValue();
            item.TakeDamage(-needDamdage);
        }
        EndSection();
    }
    public override void SetKey()
    {
        key = "FightTile";
    }
}
