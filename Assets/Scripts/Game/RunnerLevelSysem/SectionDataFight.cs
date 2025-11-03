using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "FightSection", menuName = "ScriptableObjects/FightSection")]
public class SectionDataFight : SectionData
{
    public List<EnemyWave> LevelEnemies;
    public override LevelSection CreateMono()
    {
        // FightSection section = new FightSection();
        GameObject newOBj = new GameObject();
        FightSection section = newOBj.AddComponent<FightSection>();
        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.LevelEnemies = LevelEnemies;
        return section;
    }
    public int EnemyWave = 1;
    public int EnemyCount = 5;
    public override void FillYourSelf()
    {
#if UNITY_EDITOR
        levelTiles = new List<Tile>();
        LevelEnemies = new List<EnemyWave>();
        List<Animal> AllEnemies = new List<Animal>();
        Debug.Log("Fire Section");
        // var loading = Addressables.LoadAssetAsync<GameObject>("default");
        // var loadAnimals = Addressables.LoadAssetsAsync<GameObject>("Animal", (obj) =>
        // {
        //     AllEnemies.Add(obj.GetComponent<Animal>());
        // });
        // loading.WaitForCompletion();
        // levelTiles.Add(loading.Result.GetComponent<Tile>());
        // loadAnimals.WaitForCompletion();

        var paths = AddressableHelper.GetPrefabPathssByLabel("default");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            levelTiles.Add(prefab.GetComponent<Tile>());
        }
        paths = AddressableHelper.GetPrefabPathssByLabel("Animal");
        foreach (var path in paths)
        {
            // string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            AllEnemies.Add(prefab.GetComponent<Animal>());
        }

        // levelTiles
        // loading.WaitForCompletion();
        for (int i = 0; i < EnemyWave; i++)
        {
            EnemyWave newWave = new EnemyWave();
            for (int j = 0; j < EnemyCount; j++)
            {
                newWave.EnemyPF.Add(AllEnemies[Random.Range(0, AllEnemies.Count)]);
            }
            LevelEnemies.Add(newWave);
        }
        SaveChanges();
#endif
    }
}
