using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeadItemData", menuName = "Inventory/HeadItemData")]
public class HeadItemData : BaseItemData
{
    public Vector3 wearPos = Vector3.up * 2;
    public override void Wear(Player player)
    {
        GameObject insOBj = Instantiate(pf, Vector3.zero, Quaternion.identity, player.head);
        insOBj.transform.localPosition = wearPos;
        player.HeadObj = insOBj;

    }
    public override void Unwear(Player player)
    {
        if (player.HeadObj)
        {
            Destroy(player.HeadObj);
        }
    }
}
