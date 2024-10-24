using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;
using Random = UnityEngine.Random;

// [CreateAssetMenu(fileName = "FightSection", menuName = "ScriptableObjects/FightSection")]
[Serializable]
public class FightSection : BaseSection
{
    public override SectionType SectionType => SectionType.Fight;


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
        FunctionTimer.WaitUntilAndCall(curLevel, () => Z.Player.transform.position.z > ztoTest, () => { OnFightSectionEnter(this, EventArgs.Empty); });
    }
    public override void UpdateSection()
    {
        bool isNearEndofLand = curLevel.player.transform.position.z > curLevel.nextSpawnPosition.z - 50;
        if (isNearEndofLand)
        {
            Tile tileToIns = levelTiles[Random.Range(0, levelTiles.Count)];
            curLevel.SpawnTile(tileToIns);
        }
    }
    public override void EndSection()
    {
        Debug.Log("End Fight " + EnemyWaveIdx);
        base.EndSection();
        curLevel.player.ChangeState(PlayerState.Obs);
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
        // Tile casted = (Tile)sender;
        // casted.OnTileEnter -= OnFightSectionEnter;
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
                Enemy insEnemy = GameObject.Instantiate(InsEnems[i], playerParent.position + Vector3.forward * 20 + RandomPosXZ, Quaternion.Euler(0, 180, 0), playerParent);
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
            NextorEnd();
        }
    }

    public void NextorEnd()
    {
        if (HasNextWave(EnemyWaveIdx))
        {
            Debug.Log("Next Wave");
            EnemyWaveIdx++;
            InsEnemsBeforePlayer();
        }
        else
        {

            EndSection();
        }
    }
    public override void SetKey()
    {
        key = "default";
    }
    public override async Task LoadNGenerateSelf()
    {
        await base.LoadNGenerateSelf();

        List<EnemyWave> waves = new List<EnemyWave>();
        List<Animal> AllEnemyPF = new List<Animal>();

        // Debug.Log("Loading assets...");

        // Load assets asynchronously
        var asynOperation = Addressables.LoadAssetsAsync<GameObject>("Animal", (Enemy) =>
        {
            if (Enemy != null && Enemy.GetComponent<Animal>() != null)
            {
                AllEnemyPF.Add(Enemy.GetComponent<Animal>());
            }
            else
            {
                Debug.LogError("Failed to get Animal component from loaded asset.");
            }
        });

        await asynOperation.Task;

        // Check if the asset loading succeeded
        if (asynOperation.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Asset loading failed.");
            return; // Exit early if loading fails
        }

        // Debug.Log("Assets loaded. Total Enemy prefabs: " + AllEnemyPF.Count);

        if (AllEnemyPF.Count == 0)
        {
            Debug.LogError("No enemy prefabs were loaded. Cannot proceed.");
            return; // Exit early if no enemies were loaded
        }

        // Outer loop to create enemy waves
        for (int i = 0; i < 1; i++)
        {
            EnemyWave newWave = new EnemyWave();

            // Debug.Log("Creating wave " + (i + 1));

            // Inner loop to populate enemy prefabs in the wave
            for (int j = 0; j < 3; j++)
            {
                // Randomly select an enemy prefab
                Enemy randomEnemy = AllEnemyPF[Random.Range(0, AllEnemyPF.Count)];
                newWave.EnemyPF.Add(randomEnemy);
                // Debug.Log("randomEnemy " + (j + 1) + " added.");
            }

            LevelEnemies.Add(newWave);
            Debug.Log("Wave " + (i + 1) + " created.");
        }

        Debug.Log("Finished creating all waves.");
    }

}
