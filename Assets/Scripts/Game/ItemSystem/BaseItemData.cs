using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItemData : ScriptableObject
{
    public GameObject pf;
    public string itemName;
    public Sprite Icon;
    public WhereSlot Where;
    public float AddArmor;
    public float AddHealth;
    public float AddDamage;
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
}
