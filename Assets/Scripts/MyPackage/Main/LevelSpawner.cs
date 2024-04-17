using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using ZPackage.Utility;

namespace ZPackage
{
    public class LevelSpawner : GenericSingleton<LevelSpawner>
    {
        public Tile[] tilePrefabs; // Array of tile prefabs to spawn
        public Transform player; // Reference to the player's transform
        float spawnDistance = 50f; // Distance ahead of the player to spawn new tiles
        private float nextSpawnPosition = 0f; // Position to spawn the next tile
        public List<Tile> SpawnedTiles = new List<Tile>();
        public SpawnTileType Type = SpawnTileType.Random;
        Tile lastSpawnedTile;

        private void Start()
        {
            for (int i = 0; i < 5; i++)
            {
                SpawnTile(0);
            }
        }

        private void Update()
        {
            // Calculate the next position to spawn a tile
            float playerPosition = player.position.z;
            if (playerPosition > nextSpawnPosition - 150)
            {
                // Spawn a new tile
                SpawnTile(Type);

            }
        }
        private void SpawnTile(SpawnTileType type)
        {
            int index = 0;
            if (type == SpawnTileType.None)
            {

                index = 0;
            }
            else if (type == SpawnTileType.Random)
            {
                index = Random.Range(0, tilePrefabs.Length);
            }
            else if (type == SpawnTileType.Jump)
            {
                index = 1;
            }
            // Randomly select a tile prefab from the array

            // Spawn the selected tile prefab at the next spawn position
            SpawnTile(index);
        }

        private void SpawnTile(int index = -1)
        {
            Tile tilePrefab;
            if (index != -1 && index < tilePrefabs.Length)
            {

                tilePrefab = tilePrefabs[index];
            }
            else
            {
                tilePrefab = tilePrefabs[Random.Range(0, tilePrefabs.Length)];
            }
            // Randomly select a tile prefab from the array

            // Spawn the selected tile prefab at the next spawn position
            SpawnTile(tilePrefab);
        }

        private void SpawnTile(Tile tilePrefab)
        {
            Tile tile = Instantiate(tilePrefab, new Vector3(0, 0, nextSpawnPosition), Quaternion.identity, transform);
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
            nextSpawnPosition += spawnDistance;
            if (SpawnedTiles.Count > 10)
            {
                Destroy(SpawnedTiles[0].gameObject);
                SpawnedTiles.RemoveAt(0);
            }
        }
    }
}
public enum SpawnTileType
{
    None, Random, Jump
}

