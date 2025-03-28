using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public abstract class BaseItemUI : MonoBehaviour, IUpgradeAble, ISaveAble
{
    public BaseItemData data;
    public string ID;
    public int level = 1;
    public UISlot currentSlot;
    public void Start()
    {
        SetIcon(data.Icon);
    }
    public virtual void Upgrade()
    {
        // if (level < data.AddDamage.Count)
        Debug.Log("upgradeable is " + data.IsUpgradeAble(level));
        Debug.Log("lvl is " + level);
        if (data.IsUpgradeAble(level))
        {
            level++;
            SaveData();
            Debug.Log("Upgraded " + level);
        }
    }


    public virtual List<GameObject> InstantiateNeededItem(IItemEquipper itemEquipper = null)
    {
        Debug.Log("Base Instantiate Called Doing Nothing");
        return new List<GameObject>();
    }

    internal string GetLevel()
    {
        return level + "/" + data.AddDamage.Count;
    }
    public float GetDamdage()
    {
        return data.BaseDamage + data.AddDamage[level - 1];
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt(ID, level);
    }
    public void RetrieveData()
    {
        level = PlayerPrefs.GetInt(ID, 1);
    }
    public void SetIcon(Sprite icon)
    {
        GetComponent<Image>().sprite = icon;
    }
    public List<KeyValuePair<string, string>> GetInfo()
    {
        string info = data.Info;

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
        int additionalDamage = data.AddDamage[level - 1];
        Debug.Log("lvl " + (level - 1) + " " + data.AddDamage.Count);
        int additionalSpeed = data.AddSpeed[level - 1];

        description = description.Replace("{armor}", data.AddArmor[level - 1].ToString())
                    .Replace("{health}", data.AddHealth[level - 1].ToString())
                    .Replace("{damage}", (data.BaseDamage + additionalDamage).ToString())
                    .Replace("{speed}", (data.BaseSpeed + additionalSpeed).ToString())
                    .Replace("{range}", data.BaseRange.ToString())
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

    public EquipData InstantiateImtems(IItemEquipper player)
    {
        throw new NotImplementedException();
    }
}
