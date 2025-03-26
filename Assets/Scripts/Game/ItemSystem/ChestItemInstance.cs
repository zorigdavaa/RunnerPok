using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ChestItemInstance : ItemInstance
{
    public Vector3 wearPos = Vector3.up * 2;
    public override EquipData EquipItem(IItemEquipper itemEquipper)
    {
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, itemEquipper.GetChest());
        insOBj.transform.localPosition = wearPos;
        base.EquipItem();
        EquipData equipData = new EquipData();
        equipData.InstantiatedObjects.Add(insOBj);
        return equipData;
    }
}
