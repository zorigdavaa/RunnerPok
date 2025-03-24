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
    public override void EquipItem(IItemEquipper itemEquipper)
    {

        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(Rot), Z.Player.rightFoot);
        insOBj.transform.localPosition = RightFootPos;
        Z.Player.RightFootObj = insOBj;
        GameObject Left = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(RotLeft), Z.Player.leftFoot);
        Left.transform.localPosition = LeftFootPos;
        Z.Player.LeftFootObj = Left;
        base.EquipItem();
    }
    public override void UnEquipItem(IItemEquipper itemEquipper)
    {
        if (Z.Player.RightFootObj)
        {
            Destroy(Z.Player.RightFootObj);
            Destroy(Z.Player.LeftFootObj);
        }
        base.UnEquipItem(itemEquipper);
    }
}
