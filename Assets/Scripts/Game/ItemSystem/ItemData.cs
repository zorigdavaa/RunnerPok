using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite Icon;
    public int BaseDamage;
    public float BaseSpeed = 1;
    public float BaseRange = 1;//calculated in second
    public List<int> AddDamage;
    public List<int> AddSpeed;
    public GameObject pf;
    // public BaseItemUI pfUI;
    public WhereSlot Where;
    public DamageData damageData;

    public virtual void Wear(Player player)
    {
        // throw new NotImplementedException();
    }
}
