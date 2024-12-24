using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : BaseItemData
{
    // public BaseItemUI pfUI;

    public DamageData damageData;

    public override bool IsUpgradeAble(int level)
    {
        return level < AddDamage.Count;
    }
}
