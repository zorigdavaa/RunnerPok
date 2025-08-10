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
        public bool IsLevelReady = false;
        async Task Start()
        {
            GameManager.Instance.OnGamePlay += OnGamePlay;
            Destroy(transform.GetChild(0).gameObject, 10);
            // if (LastInstLvl)
            // {
            //     pos = LastInstLvl.nextSpawnPosition;
            //     LastInstLvl.DestSelf();
            // }
            // for (int i = 0; i < 5; i++)
            // {
            //     SpawnTile(0);
            // }
            int lvlIndex = GameManager.Instance.Level - 1;
            // SpawnLevel(Levels[0]);
            if (lvlIndex < Levels.Count)
            {
                LastInstLvl = SpawnLevel(Levels[lvlIndex]);
            }
            else
            {
                // Level level = GenerateLevel();
                Task<Level> task = GenerateLevel();

                await task;
                LastInstLvl = task.Result;
            }
            IsLevelReady = true;
        }

        private void OnGamePlay(object sender, EventArgs e)
        {

            StartCoroutine(WaitAndStartCor());
            IEnumerator WaitAndStartCor()
            {
                // Debug.Log("WaitAndStartCor");
                yield return new WaitUntil(() => IsLevelReady == true);
                LastInstLvl.StartNewSection();
            }
        }

        private Level SpawnLevel(Level level)
        {
            // if (LastInstLvl)
            // {
            //     pos = LastInstLvl.nextSpawnPosition;
            //     LastInstLvl.DestSelf();
            // }
            return Instantiate(level, pos, Quaternion.identity, transform);
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
            // new SecPattern()
            // {
            //     sectionTypes = new List<SectionType>()
            //     {
            //          SectionType.Choose,SectionType.Normal,SectionType.Fight, SectionType.Choose,SectionType.Normal,SectionType.Fight
            //     },

            // },
        };

    }
}
public enum SpawnTileType
{
    None, Random, Jump
}

