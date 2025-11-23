using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage;
using Random = UnityEngine.Random;


//Complex Section
public class CSection : LevelSection
{
    public override SectionType SectionType => SectionType.Complex;
    public List<GameObject> Obstacles;
    public GameObject UpHillObs;
    public GameObject Coin;
    public List<GameObject> Boosters;

    public override void StartSection(Level level)
    {
        Reset();
        base.StartSection(level);
        EnterThisSection();
    }
    int NumberOfTiles = 20;
    //Todo Inspos is not working when Diogonal tile

    Vector3 insPos;

    // Update is called once per frame
    void Update()
    {

    }
    private void EnterThisSection()
    {
        Debug.Log("eNTER Complex Section");
        float ztoTest = curLevel.lastSpawnedTile.end.transform.position.z;
        FunctionTimer.WaitUntilAndCall(curLevel, () => Z.Player.transform.position.z > ztoTest, () => { OnObsSectionEnter(this, EventArgs.Empty); });
    }
    public override void UpdateSection()
    {
        bool isNearEndofLand = curLevel.player.transform.position.z > curLevel.nextSpawnPosition.z - Z.TileDistance;
        if (isNearEndofLand)
        {
            Tile tileToIns;
            if (!levelTiles.Any())
            {

                tileToIns = curLevel.BaseTilePf;
            }
            else
            {
                tileToIns = levelTiles[Random.Range(0, levelTiles.Count)];
            }
            curLevel.SpawnTile(tileToIns);
        }
        bool isNearOfObs = curLevel.player.transform.position.z > insPos.z - 70;
        if (isNearOfObs)
        {
            if (NumberOfTiles > 0)
            {
                SpawnObs();
            }
            else
            {
                EndSection();
            }
        }
    }

    void OnObsSectionEnter(object sender, EventArgs e)
    {
        // InsObsBeforePlayer();
    }
    public override void EndSection()
    {
        curLevel.NextNearPlayer();
        curLevel.player.ChangeState(PlayerState.Obs);
        base.EndSection();
    }
    private void SpawnObs()
    {

        // GameObject obs = Obstacles[index];

        GameObject insGO = Instantiate(UpHillObs, insPos, Quaternion.identity, curLevel.transform);
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
