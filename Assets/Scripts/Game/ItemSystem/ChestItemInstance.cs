using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ChestItemInstance : ItemInstance
{
    public Vector3 wearPos = Vector3.up * 2;
    public override void EquipItem()
    {
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, Z.Player.chest);
        insOBj.transform.localPosition = wearPos;

    }
}
