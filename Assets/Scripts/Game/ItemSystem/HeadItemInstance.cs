using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class HeadItemInstance : ItemInstance
{
    public Vector3 wearPos = Vector3.up * 2;
    public override List<GameObject> InstantiateNeededItem(IItemEquipper itemEquipper = null)
    {
        if (itemEquipper == null)
        {
            itemEquipper = Z.Player;
        }
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, itemEquipper.GetHeadTransform());
        insOBj.transform.localPosition = wearPos;
        // Z.Player.HeadObj = insOBj;
        insOBj.GetComponent<BaseEquipedItem>().ItemInstance = this;
        List<GameObject> equipData = new List<GameObject>
        {
            insOBj
        };
        return equipData;
    }
}
