using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FootItemData", menuName = "Inventory/FootItemData")]
public class FootItemdata : BaseItemData
{
    public Vector3 RightFootPos = Vector3.up * 0.1f;
    public Vector3 LeftFootPos = Vector3.up * 0.1f;
    public Vector3 Rot = Vector3.up * 0.1f;
    public Vector3 RotLeft = Vector3.up * 0.1f;
    public override void Wear(Player player)
    {
        GameObject insOBj = Instantiate(pf, Vector3.zero, Quaternion.Euler(Rot), player.rightFoot);
        insOBj.transform.localPosition = RightFootPos;
        player.RightFootObj = insOBj;
        GameObject Left = Instantiate(pf, Vector3.zero, Quaternion.Euler(RotLeft), player.leftFoot);
        Left.transform.localPosition = LeftFootPos;
        player.LeftFootObj = Left;

    }
    public override void Unwear(Player player)
    {
        if (player.RightFootObj)
        {
            Destroy(player.RightFootObj);
            Destroy(player.LeftFootObj);
        }
    }
}
