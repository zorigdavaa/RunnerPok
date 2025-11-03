using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "SkillChooseSec", menuName = "ScriptableObjects/Skillsec")]
public class SecDataChoose : SectionData
{
    public override LevelSection CreateMono()
    {
        ChooseSection section;
        GameObject newSection = new GameObject();
        section = newSection.AddComponent<ChooseSection>();
        // Common properties assigned after switch
        section.levelTiles = levelTiles;
        section.SectionEnd = SectionEnd;
        section.SectionStart = SectionStart;

        return section;
    }
    public override void FillYourSelf()
    {
#if UNITY_EDITOR
        levelTiles = new List<Tile>();
        Debug.Log("Fire Section");

        var paths = AddressableHelper.GetPrefabPathssByLabel("default");
        GameObject prefab = (paths.Count > 0) ? AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]) : null;

        // var loading = Addressables.LoadAssetAsync<GameObject>("default");
        // loading.WaitForCompletion();
        // levelTiles.Add(loading.Result.GetComponent<Tile>()); 
        levelTiles.Add(prefab.GetComponent<Tile>());

        paths = AddressableHelper.GetPrefabPathssByLabel("SkillChose");
        prefab = (paths.Count > 0) ? AssetDatabase.LoadAssetAtPath<GameObject>(paths[0]) : null;

        // var load2 = Addressables.LoadAssetAsync<GameObject>("SkillChose");
        // load2.WaitForCompletion();
        // SectionStart = load2.Result.GetComponent<Tile>();
        SectionStart = prefab.GetComponent<Tile>();
        SaveChanges();
#endif
    }
}
