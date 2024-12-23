using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public abstract class BaseItemUI : MonoBehaviour, IUpgradeAble
{
    public BaseItemData data;
    public string ID;
    public int level = 1;
    public UISlot currentSlot;
    public virtual void Upgrade()
    {
        Debug.Log("Base Upgrade");
    }
    public virtual void ShowInfo()
    {

    }

    public virtual void EquipItem()
    {
        // Debug.Log("Eqiopped " + data.name);
        switch (data.Where)
        {
            case WhereSlot.Hand: Z.Player.HandItem = (ItemData)data; break;
            case WhereSlot.OtherHand:
                if (data is OffHandItem castData)
                {
                    Z.Player.OffHandItem = castData;
                }
                break;
            case WhereSlot.Chest:
                Z.Player.ChestItem = (ChestItemData)data;
                break;
            case WhereSlot.Foot:
                Z.Player.FootItem = (FootItemdata)data;
                break;
            case WhereSlot.Head:
                Z.Player.HeadItem = (HeadItemData)data;
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
}
