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
    [TextArea] public string Desc;
    [TextArea] public string Info;
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
        return level < 10;
    }
    public string GetDescription()
    {
        string description = Desc;

        // Replace placeholders dynamically
        description = Convert(description);


        return description;
    }
    public List<KeyValuePair<string, string>> GetInfo()
    {
        string info = Info;

        // Replace placeholders dynamically
        info = Convert(info);
        List<KeyValuePair<string, string>> result = new List<KeyValuePair<string, string>>();
        foreach (var line in info.Split('\n'))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('=');
            if (parts.Length == 2)
            {
                string key = parts[0].Trim();
                string value = parts[1].Trim();
                // result[key] = value;
                result.Add(new KeyValuePair<string, string>(key, value));
            }
        }
        return result;
    }

    public virtual string Convert(string description)
    {
        description = description.Replace("{armor}", AddArmor.ToString())
            .Replace("{health}", AddHealth.ToString())
        ;
        return description;

        // description = Regex.Replace(description, @"{(\w+)}", match =>
        // {
        //     string key = match.Groups[1].Value;

        //     return key switch
        //     {
        //         "Armor" => AddArmor.ToString(),
        //         "Health" => AddHealth.ToString(), // Add more cases as needed
        //         _ => match.Value // Return original placeholder if no match
        //     };
        // });
        // return description;
    }
}
