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

    public virtual void EquipItem()
    {
        // Debug.Log("Eqiopped " + data.name);
        switch (data.Where)
        {
            case WhereSlot.Hand: Z.Player.HandItem = (ItemInstance)this; break;
            case WhereSlot.OtherHand:
                if (this is OffHandItemInstance castData)
                {
                    Z.Player.OffHandItem = castData;
                }
                break;
            case WhereSlot.Chest:
                Z.Player.ChestItem = (ChestItemInstance)this;
                break;
            case WhereSlot.Foot:
                Z.Player.FootItem = (FootItemInstance)this;
                break;
            case WhereSlot.Head:
                Z.Player.HeadItem = (HeadItemInstance)this;
                break;
            default: break;
        }
    }
    public virtual void UnEquipItem()
    {
        // Debug.Log("Unequiped " + data.name);
        switch (data.Where)
        {
            case WhereSlot.Hand: Z.Player.HandItem = null; break;
            case WhereSlot.OtherHand:
                if (this is OffHandItemInstance castData)
                {
                    Z.Player.OffHandItem = null;
                }
                break;
            case WhereSlot.Chest:
                Z.Player.ChestItem = null;
                break;
            case WhereSlot.Foot:
                Z.Player.FootItem = null;
                break;
            case WhereSlot.Head:
                Z.Player.HeadItem = null;
                break;
            default: break;
        }
    }

    internal string GetLevel()
    {
        return level + "/" + data.AddDamage.Count;
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
        int additionalDamage = level > 1 ? data.AddDamage[level - 2] : 0;
        int additionalSpeed = level > 1 ? data.AddSpeed[level - 2] : 0;

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
}
