using System.Collections;
using System.Collections.Generic;
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
        levelTiles = new List<Tile>();
        Debug.Log("Fire Section");
        var loading = Addressables.LoadAssetAsync<GameObject>("default");
        loading.WaitForCompletion();
        levelTiles.Add(loading.Result.GetComponent<Tile>());
        var load2 = Addressables.LoadAssetAsync<GameObject>("SkillChose");
        load2.WaitForCompletion();
        SectionStart = load2.Result.GetComponent<Tile>();
    }
}
