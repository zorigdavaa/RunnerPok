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
    public float AddArmor;
    public float AddHealth;
    [TextArea]
    public string Desc;
    public virtual void Wear(Player player)
    {
        throw new NotImplementedException();
    }
    public virtual void Unwear(Player player)
    {
        throw new NotImplementedException();
    }

    public virtual bool IsUpgradeAble(int level)
    {
        throw new NotImplementedException();
    }
    public virtual string GetDescription()
    {
        string description = Desc;

        // Replace placeholders dynamically
        description = Regex.Replace(description, @"{(\w+)}", match =>
        {
            string key = match.Groups[1].Value;

            return key switch
            {
                "Armor" => AddArmor.ToString(),
                "Health" => AddHealth.ToString(), // Add more cases as needed
                _ => match.Value // Return original placeholder if no match
            };
        });

        return description;
    }
}
