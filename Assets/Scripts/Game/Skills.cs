using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage;

public class Skills : GenericSingleton<Skills>
{
    public List<Skill> AllSkills;
    public List<ChooseSkill> ChooseSkills;
    // Start is called before the first frame update
    void Start()
    {
        Show3Skills();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show3Skills()
    {
        foreach (var choose in ChooseSkills)
        {
            choose.SetSkill(AllSkills[Random.Range(0, AllSkills.Count)]);
        }
        gameObject.SetActive(true);
    }

    internal void PlayerChosen(ChooseSkill chooseSkill)
    {
        gameObject.SetActive(false);
    }
}
