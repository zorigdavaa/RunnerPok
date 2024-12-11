using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChestItemData", menuName = "Inventory/ChesmItemData")]
public class ChestItemData : ItemData
{
    Vector3 wearPos = Vector3.up * 2;
    public override void Wear(Player player)
    {
        GameObject insOBj = Instantiate(pf, Vector3.zero, Quaternion.identity, player.transform);
        insOBj.transform.localPosition = wearPos;

    }
}
