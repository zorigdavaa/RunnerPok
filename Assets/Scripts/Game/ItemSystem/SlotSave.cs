using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotSave
{
    // public List<string> EquipedNames;
    // public List<string> UnEquipedNames;
    public List<ItemSaveData> EquipDataNew;
    public List<ItemSaveData> UneqiupDataNew;
    public SlotSave()
    {
        // EquipedNames = new List<string>();
        // UnEquipedNames = new List<string>();
        EquipDataNew = new List<ItemSaveData>();
        UneqiupDataNew = new List<ItemSaveData>();
    }
}

[System.Serializable]
public class ItemSaveData
{
    public string dataName;
    public string ID;
    public ItemSaveData(string Id, string dataname)
    {
        dataName = dataname;
        ID = Id;
    }
    public ItemSaveData()
    {
        dataName = "";
        ID = "";
    }
}
