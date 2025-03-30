using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EquipData : Object
{
    public ItemInstanceUI item;
    public List<GameObject> InstantiatedObjects = new List<GameObject>();
    // public WearPosData wearPosData;
}
