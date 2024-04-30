using System;
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
        public List<Level> Levels;
        public Level LastInstLvl;
        Vector3 pos = Vector3.zero;

        private void Start()
        {
            // for (int i = 0; i < 5; i++)
            // {
            //     SpawnTile(0);
            // }
            SpawnLevel(Levels[0]);
            GameManager.Instance.OnGamePlay += OnGamePlay;
        }

        private void OnGamePlay(object sender, EventArgs e)
        {
            LastInstLvl.StartNewSection();
        }

        private void SpawnLevel(Level level)
        {
            if (LastInstLvl)
            {
                pos = LastInstLvl.nextSpawnPosition;
                LastInstLvl.DestSelf();
            }
            LastInstLvl = Instantiate(level, pos, Quaternion.identity, transform);
        }
    }
}
public enum SpawnTileType
{
    None, Random, Jump
}

