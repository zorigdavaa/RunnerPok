using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;
using Random = UnityEngine.Random;

public class ObsSection : LevelSection
{
    public List<GameObject> Obstacles;
    public bool HasNextObs => Obstacles.Count > index;

    // implement this section this is runtime baseTile spawner and instantiate obstacle based on situation
    public override void StartSection(Level level)
    {
        Reset();
        base.StartSection(level);
        // Tile tileToIns = levelTiles[0];
        // // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
        // Tile tile = level.SpawnTile(tileToIns);
        // tile.OnTileEnter += OnFightSectionEnter;
        insPos = curLevel.nextSpawnPosition - Vector3.forward * 40;
        EnterThisSection();
        SpawnObs();
    }
    int index = 0;
    Vector3 insPos;
    private void SpawnObs()
    {

        GameObject obs = Obstacles[index];
        GameObject insGO = GameObject.Instantiate(obs, insPos, Quaternion.identity, curLevel.transform);
        ObsData data = insGO.GetComponent<ObsData>();
        if (data)
        {
            insPos += (Vector3.forward * data.ownLengh);
            insPos += Vector3.forward * 10;
        }
        else
        {
            insPos += Vector3.forward * 25;
        }
        index++;
    }

    private void EnterThisSection()
    {
        Debug.Log("eNTER ObsSection");
        float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        FunctionTimer.WaitUntilAndCall(curLevel, () => Z.Player.transform.position.z > ztoTest, () => { OnObsSectionEnter(this, EventArgs.Empty); });
    }
    public override void UpdateSection()
    {
        bool isNearEndofLand = curLevel.player.transform.position.z > curLevel.nextSpawnPosition.z - Z.TileDistance;
        if (isNearEndofLand)
        {
            Tile tileToIns = curLevel.BaseTilePf;
            curLevel.SpawnTile(tileToIns);
        }
        bool isNearOfObs = curLevel.player.transform.position.z > insPos.z - 70;
        if (isNearOfObs)
        {
            if (HasNextObs)
            {
                SpawnObs();
            }
            else
            {
                EndSection();
            }
        }
    }
    public override void EndSection()
    {
                curLevel.NextNearPlayer();
        curLevel.player.ChangeState(PlayerState.Obs);
        base.EndSection();
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
    public override void SetKey()
    {
        key = "Obstacle";
    }
    public override async Task LoadNGenerateSelf()
    {
        // await base.LoadNGenerateSelf();
        List<GameObject> AllObs = new List<GameObject>();
        Obstacles = new List<GameObject>();
        Debug.Log("Loading assets...");
        SetKey();
        // Load assets asynchronously
        var asynOperation = Addressables.LoadAssetsAsync<GameObject>(key, (Obs) =>
        {

            // Debug.LogError("Asset loading Obstacle " + Obs.name);
            AllObs.Add(Obs);

        });

        await asynOperation.Task;
        int ObsCount = 8;
        for (int i = 0; i < ObsCount; i++)
        {

            Obstacles.Add(AllObs[Random.Range(0, AllObs.Count)]);
        }

        // Check if the asset loading succeeded
        if (asynOperation.Status != UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Asset loading failed.");
            return; // Exit early if loading fails
        }

        // Debug.Log("Assets loaded. Total Enemy prefabs: " + AllEnemyPF.Count);

        if (AllObs.Count == 0)
        {
            Debug.LogError("No enemy prefabs were loaded.");
            return; // Exit early if no enemies were loaded
        }
    }
}
