using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
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

        private void Start()
        {
            // for (int i = 0; i < 5; i++)
            // {
            //     SpawnTile(0);
            // }
            int lvlIndex = GameManager.Instance.Level - 1;
            // SpawnLevel(Levels[0]);
            SpawnLevel(Levels[lvlIndex]);
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
        public Level GenerateLevel()
        {
            Random.InitState(GameManager.Instance.Level);
            Level Level = new Level();
            int SectionCount = 4;
            for (int i = 0; i < SectionCount; i++)
            {
                LevelSection section = AvailAbleSection[Random.Range(0, AvailAbleSection.Count - 1)];//availLast Boss
                section.GenerateSelf();
                // int SectionTileCount = 5;
                // for (int j = 0; j < SectionTileCount; j++)
                // {
                //     Tile Tile = AllTiles[Random.Range(0, AllTiles.Count)];
                //     section.levelTiles.Add(Tile);
                // }
                Level.LevelObjects.Add(section);
            }
            return Level;
        }
        public IEnumerable<Type> GetAllDerivedClassOf(Type type)
        {
            return Assembly.GetAssembly(type).GetTypes().Where(x => x.IsSubclassOf(type) && !x.IsAbstract);

        }
        List<LevelSection> AvailAbleSection = new List<LevelSection>()
            {
                new FightSection()
                ,new CollectSection()
                ,new LevelSection()
                // ,new BossSection()
            };
    }
}
public enum SpawnTileType
{
    None, Random, Jump
}

