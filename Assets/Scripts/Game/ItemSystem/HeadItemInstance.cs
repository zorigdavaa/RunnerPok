using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class HeadItemInstance : ItemInstance
{
    public Vector3 wearPos = Vector3.up * 2;
    public override EquipData EquipItem(IItemEquipper itemEquipper = null)
    {
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, itemEquipper.GetHeadTransform());
        insOBj.transform.localPosition = wearPos;
        // Z.Player.HeadObj = insOBj;
        insOBj.GetComponent<BaseEquipedItem>().ItemInstance = this;
        base.EquipItem();
        EquipData equipData = new EquipData();
        equipData.InstantiatedObjects.Add(insOBj);
        return equipData;
    }
    public override void UnEquipItem(IItemEquipper itemEquipper = null)
    {
        if (Z.Player.HeadObj)
        {
            Destroy(Z.Player.HeadObj);
            Z.Player.HeadObj = null;
        }
        base.UnEquipItem();
    }
}
