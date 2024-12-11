using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : BaseItemData
{
    public int BaseDamage;
    public float BaseSpeed = 1;
    public float BaseRange = 1;//calculated in second
    public List<int> AddDamage;
    public List<int> AddSpeed;

    // public BaseItemUI pfUI;

    public DamageData damageData;

    public override void Wear(Player player)
    {
        throw new NotImplementedException();
    }
    public override bool IsUpgradeAble(int level)
    {
        return AddDamage.Count < level;
    }
}
