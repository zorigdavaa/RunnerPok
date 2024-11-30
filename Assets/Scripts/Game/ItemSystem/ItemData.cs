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
    public List<int> AddDamage;
    public GameObject pf;
    public BaseItemUI pfUI;
    public WhereSlot Where;
    public DamageData damageData;
}
