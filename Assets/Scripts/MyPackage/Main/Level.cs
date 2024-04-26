using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage;

public class Level : MonoBehaviour
{
    int SecIDX = 0;
    int SecTileIDx = 0;
    public List<LevelSection> LevelObjects;
    public Player player; // Reference to the player's transform
    [SerializeField] Transform nextSpawnPosition; // Position to spawn the next tile
    bool HasNextSection => LevelObjects.Count - 1 > SecIDX;
    public Tile BaseTilePf;
    // bool CurSectionHasTile => CurrentSection.levelTiles.Count - 1 > SecTileIDx;
    // [SerializeField] LevelSection CurSection => LevelObjects[SecIDX];
    [SerializeField] LevelSection CurSection;

    public void Start()
    {
        // BaseTilePf = 
        nextSpawnPosition = transform;
        SpawnedTiles = new List<Tile>();
        player = Z.Player;
        // StartNewSection();
    }

    public void StartNewSection()
    {
        BeforSectionTiles = SpawnedTiles;
        SpawnedTiles.Clear();
        CurSection = LevelObjects[SecIDX];
        CurSection.StartSection(this);
        print("Started " + CurSection.SectionType + " " + CurSection.name);
        if (CurSection.SectionType == SectionType.Fight)
        {
            SpawnedTiles[0].OnTileEnter += OnFightSectionEnter;
        }
        else
        {
            player.GoingToFight(false);
        }
        // Tile tileToIns = CurSection.SectionStart;
        // SpawnTile(tileToIns);
    }

    private void OnFightSectionEnter(object sender, EventArgs e)
    {
        print("Start Insing");
        StartInstantiateEnemies();
    }

    public void Update()
    {
        bool isNearEndofLand = player.transform.position.z > nextSpawnPosition.position.z - 30;
        // if (isNearEndofLand && CurSectionHasTile)
        if (CurSection == null)
        {
            if (isNearEndofLand)
            {
                // Tile tileToIns = CurSection.levelTiles[SecTileIDx];
                // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
                SpawnTile(BaseTilePf);
                // SecTileIDx++;
            }
        }
        else if (CurSection.SectionType == SectionType.Obstacle)
        {
            if (isNearEndofLand && CurSection.HasNextTile(SecTileIDx))
            {
                Tile tileToIns = CurSection.levelTiles[SecTileIDx];
                SpawnTile(tileToIns);
                SecTileIDx++;
                if (!CurSection.HasNextTile(SecTileIDx))
                {
                    CurSection.Oncomplete?.Invoke(this, EventArgs.Empty);
                    CurSection.EndSection(this);
                    if (HasNextSection)
                    {
                        SecIDX++;
                        SecTileIDx = 0;
                        StartNewSection();
                    }
                    else
                    {
                        CurSection = null;
                        Z.GM.LevelComplete(this, 0);
                    }
                }
            }
        }
        else if (CurSection.SectionType == SectionType.Fight)
        {
            // if (isNearEndofLand && CurSection.HasNextTile(SecTileIDx))
            if (isNearEndofLand)
            {
                Tile tileToIns = CurSection.levelTiles[SecTileIDx];
                // Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
                SpawnTile(tileToIns);
                // SecTileIDx++;
            }
        }

        // else if (!HasNextSection && isNearEndofLand)
        // {
        //     SpawnTile();
        // }
    }
    int EnemyWaveIdx = 0;
    int RemainingEnemy = 0;
    private void StartInstantiateEnemies()
    {
        player.GoingToFight(true);
        List<Enemy> Wave = CurSection.LevelEnemies[EnemyWaveIdx].EnemyPF;
        Transform playerParent = player.GetComponent<PlayerMovement>().playerParent;
        RemainingEnemy = Wave.Count;
        print(EnemyWaveIdx + " new Wave");
        for (int i = 0; i < Wave.Count; i++)
        {
            Enemy insEnemy = Instantiate(Wave[i], playerParent.position + Vector3.forward * 20, Quaternion.Euler(0, 180, 0), playerParent);
            insEnemy.Ondeath += OnEnemyDeath;
        }
    }


    private void OnEnemyDeath(object sender, EventArgs e)
    {
        RemainingEnemy--;
        print("deatj " + RemainingEnemy);
        if (RemainingEnemy == 0)
        {
            if (CurSection.HasNextWave(EnemyWaveIdx))
            {
                print("Next Wave");
                EnemyWaveIdx++;
                StartInstantiateEnemies();
            }
            else
            {
                print("End Fight " + EnemyWaveIdx);
                CurSection.EndSection(this);
                if (HasNextSection)
                {
                    SecIDX++;
                    SecTileIDx = 0;
                    StartNewSection();
                }
                else
                {
                    CurSection = null;
                    Z.GM.LevelComplete(this, 0);
                }
            }
        }
    }

    // private void SpawnTile(SpawnTileType type)
    // {
    //     int index = 0;
    //     if (type == SpawnTileType.None)
    //     {

    //         index = 0;
    //     }
    //     else if (type == SpawnTileType.Random)
    //     {
    //         index = Random.Range(0, tilePrefabs.Length);
    //     }
    //     else if (type == SpawnTileType.Jump)
    //     {
    //         index = 1;
    //     }
    //     // Randomly select a tile prefab from the array

    //     // Spawn the selected tile prefab at the next spawn position
    //     SpawnTile(index);
    // }

    // private void SpawnTile(int index = -1)
    // {
    //     Tile tilePrefab;
    //     if (index != -1 && index < tilePrefabs.Length)
    //     {

    //         tilePrefab = tilePrefabs[index];
    //     }
    //     else
    //     {
    //         tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
    //     }
    //     // Randomly select a tile prefab from the array

    //     // Spawn the selected tile prefab at the next spawn position
    //     SpawnTile(tilePrefab);
    // }
    Tile lastSpawnedTile;
    public List<Tile> SpawnedTiles;
    public List<Tile> BeforSectionTiles;
    public Tile SpawnTile(Tile tilePrefab)
    {
        Tile tile = Instantiate(tilePrefab, nextSpawnPosition.position, Quaternion.identity, transform);
        if (lastSpawnedTile)
        {
            // lastSpawnedTile.Nexttile = tile;
            lastSpawnedTile.SetNextTile(tile);
        }
        lastSpawnedTile = tile;
        if (SpawnedTiles.Count > 0)
        {
            lastSpawnedTile.BeforeTile = SpawnedTiles[SpawnedTiles.Count - 1];
        }
        else if (BeforSectionTiles.Count > 0)
        {
            lastSpawnedTile.BeforeTile = BeforSectionTiles[SpawnedTiles.Count - 1];
        }
        SpawnedTiles.Add(tile);
        // nextSpawnPosition += spawnDistance;
        nextSpawnPosition = tile.end;
        if (SpawnedTiles.Count > 10)
        {
            Destroy(SpawnedTiles[0].gameObject);
            SpawnedTiles.RemoveAt(0);
        }
        return tile;
    }
}
