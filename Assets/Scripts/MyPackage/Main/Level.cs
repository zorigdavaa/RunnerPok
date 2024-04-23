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
    public Transform player; // Reference to the player's transform
    Transform nextSpawnPosition; // Position to spawn the next tile
    bool HasNextSection => LevelObjects.Count - 1 > SecIDX;
    // bool CurSectionHasTile => CurrentSection.levelTiles.Count - 1 > SecTileIDx;
    LevelSection CurSection => LevelObjects[SecIDX];

    public void Start()
    {
        nextSpawnPosition = transform;
        SpawnedTiles = new List<Tile>();
        player = Z.Player.transform;
        StartNewSection();
    }

    private void StartNewSection()
    {
        CurSection.Start(this);
        // Tile tileToIns = CurSection.SectionStart;
        // SpawnTile(tileToIns);
    }

    public void Update()
    {
        bool isNearEndofLand = player.position.z > nextSpawnPosition.position.z - 150;
        // if (isNearEndofLand && CurSectionHasTile)
        if (CurSection.SectionType == SectionType.Obstacle)
        {
            if (isNearEndofLand && CurSection.HasNextTile(SecTileIDx))
            {
                Tile tileToIns = CurSection.levelTiles[SecTileIDx];
                SpawnTile(tileToIns);
                SecTileIDx++;
                if (!CurSection.HasNextTile(SecTileIDx))
                {
                    CurSection.Oncomplete?.Invoke(this, EventArgs.Empty);
                    CurSection.End(this);
                    if (HasNextSection)
                    {
                        SecIDX++;
                        SecTileIDx = 0;
                        StartNewSection();
                    }
                }
            }
        }
        else if (CurSection.SectionType == SectionType.Fight)
        {
            if (isNearEndofLand && CurSection.HasNextTile(SecTileIDx))
            {
                Tile tileToIns = CurSection.levelTiles[SecTileIDx % CurSection.levelTiles.Count];
                SpawnTile(tileToIns);
                SecTileIDx++;

            }
        }

        // else if (!HasNextSection && isNearEndofLand)
        // {
        //     SpawnTile();
        // }
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
    List<Tile> SpawnedTiles;
    public void SpawnTile(Tile tilePrefab)
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
        SpawnedTiles.Add(tile);
        // nextSpawnPosition += spawnDistance;
        nextSpawnPosition = tile.end;
        if (SpawnedTiles.Count > 10)
        {
            Destroy(SpawnedTiles[0].gameObject);
            SpawnedTiles.RemoveAt(0);
        }
    }
}
