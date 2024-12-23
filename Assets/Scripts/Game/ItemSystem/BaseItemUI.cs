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
                if (data is OffHandItem castData)
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
        return level + "/" + data.AddArmor;
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
}
