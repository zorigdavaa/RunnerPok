using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage;
using Random = UnityEngine.Random;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class Level : MonoBehaviour
{
    public int SecIDX = 0;
    // public int SecTileIDx = 0;
    public List<BaseSection> Sections = new List<BaseSection>();
    public List<SectionData> SectionDatas = new List<SectionData>();
    public Player player; // Reference to the player's transform
    public Vector3 nextSpawnPosition; // Position to spawn the next tile
    bool HasNextSection => Sections.Count - 1 > SecIDX;
    List<AsyncOperationHandle> OperationHandles = new List<AsyncOperationHandle>();
    public Tile PlayerBeingTile;
    public SpeedUp speedUpPF;
    public Tile BaseTilePf;
    [SerializeField] BaseSection CurSection;
    public float HealthMultiplier = 1;
    public float DamageMultiplier = 1;
    public void Start()
    {
        nextSpawnPosition = transform.position;
        SpawnedTiles = new List<Tile>();
        player = Z.Player;
        // StartNewSection();
        foreach (var item in SectionDatas)
        {
            BaseSection section = item.CreateMono();
            Sections.Add(section);
            // LevelObjects.Add(new LevelSection());
        }

    }

    // private LevelSection CreateAndGetSection(SectionData item)
    // {
    //     switch (item)
    //     {
    //         case item is SectionDataFight:
    //             FightSection section = new FightSection();
    //             section.levelTiles = item.levelTiles;
    //             section.LevelEnemies = item.LevelEnemies;
    //             return section;
    //         case SectionType.Collect: return new CollectSection();
    //         default: return new LevelSection();
    //     }
    // }

    public void Update()
    {
        // if (isNearEndofLand && CurSectionHasTile)
        if (CurSection == null && BaseTilePf != null)
        {
            bool isNearEndofLand = player.transform.position.z > nextSpawnPosition.z - 70;
            if (isNearEndofLand)
            {
                SpawnTile(BaseTilePf);
            }
        }
        else if (CurSection != null)
        {
            CurSection.UpdateSection(this);
        }

    }

    private void NextSection(object caller, EventArgs args)
    {
        CurSection.Oncomplete -= NextSection;
        if (HasNextSection)
        {
            SecIDX++;
            StartNewSection();
        }
        else
        {
            CurSection = null;
            //Todo Make it Wait Until
            FunctionTimer.WaitAndCall(this, 2, () => { Z.GM.LevelComplete(this, 0); });
            // Tile tile = SpawnTile(BaseTilePf);
            // tile.OnTileEnter += LevelComplete;

        }
    }

    private void LevelComplete(object sender, EventArgs e)
    {
        Tile tile = sender as Tile;
        // tile.OnTileEnter -= LevelComplete;
        Z.GM.LevelComplete(this, 0);
    }

    public void StartNewSection()
    {
        // SecTileIDx = 0;
        BeforSectionTiles.Clear();
        BeforSectionTiles = new List<Tile>(SpawnedTiles);
        CurSection = Sections[SecIDX];
        if (SecIDX > 0)
        {
            BaseSection PrevSection = null;
            PrevSection = Sections[SecIDX - 1];
            if (PrevSection.SectionType == SectionType.Fight)
            {
                InsSpeedUp();
            }

            // Vector3 NearPlayerPos = GetNearPlayerPos(BeforSectionTiles);
        }
        else
        {
            InsSpeedUp();
        }
        if (Z.Player.GetState() == PlayerState.Wait)
        {
            Z.Player.ChangeState(PlayerState.Obs);
        }
        CurSection.StartSection(this);
        CurSection.Oncomplete += NextSection;
        // print("Start Of From Level " + CurSection.SectionType + " " + CurSection.name);
    }

    private void InsSpeedUp()
    {
        Vector3 pos = player.transform.position + Vector3.forward * 10;
        pos.x = 0;
        SpeedUp sp = Instantiate(speedUpPF, pos, Quaternion.identity, transform);
        Destroy(sp, 10);
    }

    public Tile lastSpawnedTile;
    public List<Tile> SpawnedTiles = new List<Tile>();
    public List<Tile> BeforSectionTiles = new List<Tile>();
    public Tile SpawnTile(Tile tilePrefab)
    {
        Tile tile = Instantiate(tilePrefab, nextSpawnPosition, Quaternion.identity, transform);
        if (SpawnedTiles.Count > 0)
        {
            lastSpawnedTile.SetNextTile(tile);
            lastSpawnedTile.BeforeTile = SpawnedTiles[SpawnedTiles.Count - 1];
        }
        else if (BeforSectionTiles.Count > 0)
        {
            lastSpawnedTile.BeforeTile = BeforSectionTiles[BeforSectionTiles.Count - 1];
        }
        lastSpawnedTile = tile;
        SpawnedTiles.Add(tile);
        // nextSpawnPosition += spawnDistance;
        nextSpawnPosition = tile.end.position;
        if (SpawnedTiles.Count > 8)
        {
            Destroy(SpawnedTiles[0].gameObject);
            SpawnedTiles.RemoveAt(0);
        }
        return tile;
    }

    internal void DestSelf()
    {
        for (int i = SpawnedTiles.Count - 1; i >= 0; i--)
        {
            Destroy(SpawnedTiles[i].gameObject, i);
        }

    }

    public async Task LoadAssets()
    {
        // BaseTilePf = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Tiles/0 BaseTile.prefab").WaitForCompletion().GetComponent<Tile>();
        // speedUpPF = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Obs/SpeedUpBig.prefab").WaitForCompletion().GetComponent<SpeedUp>();
        var operation = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Tiles/0 BaseTile.prefab");
        var operation2 = Addressables.LoadAssetAsync<GameObject>("Assets/Prefabs/Obs/SpeedUpBig.prefab");
        await operation.Task;
        await operation2.Task;
        BaseTilePf = operation.Task.Result.GetComponent<Tile>();
        speedUpPF = operation2.Task.Result.GetComponent<SpeedUp>();
    }

    internal async Task GenerateSections(int sectionCount)
    {
        Sections.Clear();
        for (int i = 0; i < sectionCount; i++)
        {
            BaseSection section = AvailAbleSection[Random.Range(0, AvailAbleSection.Count - 1)];//availLast Boss
            // BaseSection section = GetRandomSectionInstance();
            await section.LoadNGenerateSelf();
            // int SectionTileCount = 5;
            // for (int j = 0; j < SectionTileCount; j++)
            // {
            //     Tile Tile = AllTiles[Random.Range(0, AllTiles.Count)];
            //     section.levelTiles.Add(Tile);
            // }
            Sections.Add(section);
            // lvl.Sections.Add(section);
        }
    }
    // List of all possible types that inherit from BaseSection
    private List<Type> sectionTypes = new List<Type>
    {
        typeof(LevelSection),
        typeof(FightSection),
        typeof(CollectSection)
        // Add more types as needed
    };

    // public BaseSection GetRandomSectionInstance()
    // {
    //     // Randomly select a type from the list
    //     Type randomType = sectionTypes[UnityEngine.Random.Range(0, sectionTypes.Count)];

    //     // Create an instance of the randomly selected type
    //     return (BaseSection)ScriptableObject.CreateInstance(randomType);
    // }
    List<BaseSection> AvailAbleSection = new List<BaseSection>()
            {
                new FightSection()
                ,new CollectSection()
                ,new LevelSection()
                // ,new BossSection()
            };
}
