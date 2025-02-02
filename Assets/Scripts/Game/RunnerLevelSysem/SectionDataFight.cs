using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "FightSection", menuName = "ScriptableObjects/FightSection")]
public class SectionDataFight : SectionData
{
    public List<EnemyWave> LevelEnemies;
    internal override BaseSection CreateMono()
    {
        FightSection section = new FightSection();

        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;
        section.LevelEnemies = LevelEnemies;
        return section;
    }
    public int EnemyWave = 1;
    public int EnemyCount = 5;
    public async override Task FillYourSelf()
    {
        levelTiles.Clear();
        LevelEnemies.Clear();
        List<Animal> AllEnemies = new List<Animal>();
        Debug.Log("Fire Section");
        var loading = Addressables.LoadAssetAsync<GameObject>("default");
        var loadAnimals = Addressables.LoadAssetsAsync<GameObject>("Animal", (obj) =>
        {
            AllEnemies.Add(obj.GetComponent<Animal>());
        });
        await loading.Task;
        levelTiles.Add(loading.Result.GetComponent<Tile>());
        await loadAnimals.Task;
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
    }
}
