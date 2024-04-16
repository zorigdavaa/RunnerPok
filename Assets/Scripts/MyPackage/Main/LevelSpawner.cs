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
        public GameObject[] tilePrefabs; // Array of tile prefabs to spawn
        public Transform player; // Reference to the player's transform
        float spawnDistance = 50f; // Distance ahead of the player to spawn new tiles
        private float nextSpawnPosition = 0f; // Position to spawn the next tile
        public List<GameObject> SpawnedTiles = new List<GameObject>();
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
                SpawnTile();

            }
        }

        private void SpawnTile(int index = -1)
        {
            GameObject tilePrefab;
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
            GameObject tile = Instantiate(tilePrefab, new Vector3(0, 0, nextSpawnPosition), Quaternion.identity, transform);
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

