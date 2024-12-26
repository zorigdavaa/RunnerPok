using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class HeadItemInstance : ItemInstance
{
    public Vector3 wearPos = Vector3.up * 2;
    public override void EquipItem()
    {
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, Z.Player.head);
        insOBj.transform.localPosition = wearPos;
        Z.Player.HeadObj = insOBj;
        insOBj.GetComponent<BaseEquipedItem>().ItemInstance = this;
        base.EquipItem();
    }
    public override void UnEquipItem()
    {
        if (Z.Player.HeadObj)
        {
            Destroy(Z.Player.HeadObj);
            Z.Player.HeadObj = null;
        }
        base.UnEquipItem();
    }
}
