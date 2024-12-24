using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class BaseItemData : ScriptableObject
{
    public GameObject pf;
    public string itemName;
    public Sprite Icon;
    public WhereSlot Where;
    [TextArea] public string Desc;
    [TextArea] public string Info;
    public int BaseDamage;
    public float BaseSpeed = 1;
    public float BaseRange = 1;//calculated in second
    public List<int> AddDamage;
    public List<int> AddSpeed;
    public List<float> AddArmor;
    public List<float> AddHealth;

    public virtual bool IsUpgradeAble(int level)
    {
        return level < 10;
    }
    public string GetDescription()
    {
        string description = Desc;

        // Replace placeholders dynamically
        // description = Convert(description);


        return description;
    }
}
