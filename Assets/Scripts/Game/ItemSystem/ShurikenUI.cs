using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShurikenUI : BaseItemUI
{

    int level = 1;
    // Start is called before the first frame update
    void Start()
    {
        // level = 
    }


    // Update is called once per frame
    void Update()
    {

    }
    public override void Upgrade()
    {
        if (level < data.AddDamage.Count)
        {
            level++;
            SaveData();
        }
    }
    public void SaveData()
    {
        PlayerPrefs.SetInt(data.name, level);
    }
    public void Equip()
    {

    }
}