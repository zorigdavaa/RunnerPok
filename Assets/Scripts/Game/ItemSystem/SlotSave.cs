using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlotSave
{
    public List<string> EquipedNames;
    public List<string> UnEquipedNames;
    public SlotSave()
    {
        EquipedNames = new List<string>();
        UnEquipedNames = new List<string>();
    }
}
