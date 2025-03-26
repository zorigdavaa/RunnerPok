using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class FootItemInstance : ItemInstance
{
    public Vector3 RightFootPos = Vector3.up * 0.1f;
    public Vector3 LeftFootPos = Vector3.up * 0.1f;
    public Vector3 Rot = Vector3.up * 0.1f;
    public Vector3 RotLeft = Vector3.up * 0.1f;
    public override EquipData InstantiateNeededItem(IItemEquipper itemEquipper)
    {
        if (itemEquipper == null)
        {
            itemEquipper = Z.Player;
        }
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(Rot), itemEquipper.GetRightFoot());
        insOBj.transform.localPosition = RightFootPos;
        // Z.Player.RightFootObj = insOBj;
        GameObject Left = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(RotLeft), itemEquipper.GetLeftFoot());
        Left.transform.localPosition = LeftFootPos;
        EquipData equipData = new EquipData();
        equipData.InstantiatedObjects.Add(insOBj);
        equipData.InstantiatedObjects.Add(Left);
        return equipData;
    }
}
