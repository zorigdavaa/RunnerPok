using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class FootItemUI : ItemInstanceUI
{
    // public Vector3 RightFootPos = Vector3.up * 0.1f;
    // public Vector3 LeftFootPos = Vector3.up * 0.1f;
    // public Vector3 Rot = Vector3.up * 0.1f;
    // public Vector3 RotLeft = Vector3.up * 0.1f;
    // public override List<GameObject> InstantiateNeededItem(IItemEquipper itemEquipper)
    // {
    //     if (itemEquipper == null)
    //     {
    //         itemEquipper = Z.Player;
    //     }

    //     // GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(Rot), itemEquipper.GetRightFoot());
    //     // insOBj.transform.localPosition = RightFootPos;
    //     // // Z.Player.RightFootObj = insOBj;
    //     // GameObject Left = Instantiate(data.pf, Vector3.zero, Quaternion.Euler(RotLeft), itemEquipper.GetLeftFoot());
    //     // Left.transform.localPosition = LeftFootPos;


    //     List<GameObject> equipData = new List<GameObject>();
    //     foreach (var item in data.WearDatas)
    //     {
    //         GameObject Left = Instantiate(item.PF, item.Position, item.GetRotation, itemEquipper.GetNeededTransform(item.where));
    //         Left.transform.localPosition = LeftFootPos;
    //         equipData.Add(Left);
    //     }

    //     return equipData;
    // }
}
