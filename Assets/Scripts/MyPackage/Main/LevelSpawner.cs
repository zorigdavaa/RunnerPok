using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using ZPackage.Utility;
using Random = UnityEngine.Random;

namespace ZPackage
{
    public class LevelSpawner : GenericSingleton<LevelSpawner>
    {
        public List<Level> Levels;
        public Level LastInstLvl;
        Vector3 pos = Vector3.zero;
        public List<Tile> AllTiles;
        private async Task Start()
        {
            Destroy(transform.GetChild(0).gameObject, 10);

            // for (int i = 0; i < 5; i++)
            // {
            //     SpawnTile(0);
            // }
            int lvlIndex = GameManager.Instance.Level - 1;
            // SpawnLevel(Levels[0]);
            if (lvlIndex < Levels.Count)
            {
                SpawnLevel(Levels[lvlIndex]);
            }
            else
            {
                // Level level = GenerateLevel();
                Task<Level> task = GenerateLevel();
                if (LastInstLvl)
                {
                    pos = LastInstLvl.nextSpawnPosition;
                    LastInstLvl.DestSelf();
                }
                await task;
                LastInstLvl = task.Result;
            }
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
        public async Task<Level> GenerateLevel()
        {
            Random.InitState(GameManager.Instance.Level);
            GameObject LevelGo = new GameObject();
            LevelGo.name = GameManager.Instance.Level + " Level";
            Level lvl = LevelGo.AddComponent<Level>();
            await lvl.LoadAssets();
            int SectionCount = 10;
            await lvl.GenerateSections(SectionCount);
            lvl.transform.SetParent(transform);
            Debug.Log("Done Level generate");
            return lvl;
        }

        public List<SecPattern> SectionsPatternData = new List<SecPattern>()
        {
            new SecPattern()
            {
                sectionTypes = new List<SectionType>()
                {
                     SectionType.Choose,SectionType.Obstacle,SectionType.Fight, SectionType.Choose,SectionType.Obstacle,SectionType.Fight
                },

            },
        };

    }
}
public enum SpawnTileType
{
    None, Random, Jump
}

