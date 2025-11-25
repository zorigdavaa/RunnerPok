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
    public GameObject Train;
    public GameObject Coin;
    public List<GameObject> Boosters;
    public List<Lane> Lanes = new List<Lane>() {
        new Lane(){ LaneXPosition = -6f, Objects = new List<GameObject>() },
        new Lane(){ LaneXPosition = 0f, Objects = new List<GameObject>() },
        new Lane(){ LaneXPosition = 6f, Objects = new List<GameObject>() }
    };

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
                SpwawnUpHill();
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
    private void SpwawnUpHill()
    {
        LanePattern pattern = (LanePattern)Random.Range(0, 5);

        // GameObject insGO = Instantiate(UpHillObs, insPos, Quaternion.identity, curLevel.transform);
        SpawnPattern(pattern, insPos.z);
        insPos += Vector3.forward * 50;
        // ObsData data = insGO.GetComponent<ObsData>();
        // if (data)
        // {
        //     insPos += (Vector3.forward * data.ownLengh);
        //     insPos += Vector3.forward * 20;
        // }
        // else
        // {
        //     insPos += Vector3.forward * 50;
        // }
    }
    void SpawnPattern(LanePattern pattern, float spawnZ)
    {
        List<GameObject> spawnedObjs = new List<GameObject>();
        switch (pattern)
        {
            case LanePattern.Single:
                spawnedObjs.Add(SpawnInLane(Random.Range(0, 3), spawnZ));
                break;

            case LanePattern.DoubleLeftCenter:
                spawnedObjs.Add(SpawnInLane(0, spawnZ));
                spawnedObjs.Add(SpawnInLane(1, spawnZ));
                break;

            case LanePattern.DoubleCenterRight:
                spawnedObjs.Add(SpawnInLane(1, spawnZ));
                spawnedObjs.Add(SpawnInLane(2, spawnZ));
                break;
            case LanePattern.DoubleLeftRight:
                spawnedObjs.Add(SpawnInLane(0, spawnZ));
                spawnedObjs.Add(SpawnInLane(2, spawnZ));
                break;

            case LanePattern.Triple:
                spawnedObjs.Add(SpawnInLane(0, spawnZ));
                spawnedObjs.Add(SpawnInLane(1, spawnZ));
                spawnedObjs.Add(SpawnInLane(2, spawnZ));
                break;
        }
        foreach (var item in spawnedObjs)
        {
            int ExtendCount = Random.Range(1, 4);
            if (Random.value < 0.3f)
            {
                item.GetComponent<UpHill>().ExtendUphill(ExtendCount);
            }
        }
    }
    // public float[] laneX = new float[] { -6f, 0f, 6f };   // Left, Center, Right

    GameObject SpawnInLane(int lane, float z, GameObject insObj = null)
    {
        if (insObj == null)
        {
            insObj = UpHillObs;
        }
        Vector3 pos = new Vector3(Lanes[lane].LaneXPosition, 0, z);
        GameObject Obj = Instantiate(insObj, pos, Quaternion.identity, curLevel.transform);
        Lanes[lane].Objects.Add(Obj);
        return Obj;
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

public enum LanePattern
{
    Single,
    DoubleLeftCenter,
    DoubleCenterRight,
    DoubleLeftRight,
    Triple
}
