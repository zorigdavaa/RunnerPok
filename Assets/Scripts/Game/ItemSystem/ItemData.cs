using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
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
    public override string Convert(string description)
    {

        description = description.Replace("{armor}", AddArmor.ToString())
                    .Replace("{health}", AddHealth.ToString())
                    .Replace("{damage}", BaseDamage.ToString())
                    .Replace("{speed}", BaseSpeed.ToString())
                    .Replace("{range}", BaseRange.ToString())
;
        return description;
        // description = Regex.Replace(description, @"{(\w+)}", match =>
        // {
        //     string key = match.Groups[1].Value;

        //     return key switch
        //     {
        //         "Armor" => AddArmor.ToString(),
        //         "Health" => AddHealth.ToString(), // Add more cases as needed
        //         "damage" => BaseDamage.ToString(), // Add more cases as needed
        //         "speed" => BaseSpeed.ToString(), // Add more cases as needed
        //         "range" => BaseRange.ToString(), // Add more cases as needed
        //         _ => match.Value // Return original placeholder if no match
        //     };
        // });
        // return description;
    }
}
