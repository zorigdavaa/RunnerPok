using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ZPackage;
using System;
using Random = UnityEngine.Random;

public class Skills : GenericSingleton<Skills>
{
    public List<BaseSkill> AllSkills;
    public List<ChooseSkill> ChooseSkills;
    public List<BaseSkill> ShowingSkills;
    public EventHandler OnChoose;
    // Start is called before the first frame update
    void Start()
    {
        // Show3Skills();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Show3Skills()
    {
        List<BaseSkill> copyAllSkill = new List<BaseSkill>(AllSkills);
        ShowingSkills = new List<BaseSkill>();
        foreach (var choose in ChooseSkills)
        {
            BaseSkill tobeShown = copyAllSkill[Random.Range(0, copyAllSkill.Count)];
            copyAllSkill.Remove(tobeShown);
            ShowingSkills.Add(tobeShown);
            choose.SetSkill(tobeShown);
        }
        gameObject.SetActive(true);
        // Time.timeScale = 0;
    }

    internal void PlayerChosen(ChooseSkill chooseSkill)
    {
        OnChoose?.Invoke(chooseSkill, EventArgs.Empty);
        gameObject.SetActive(false);
        // Time.timeScale = 1;
    }
}
