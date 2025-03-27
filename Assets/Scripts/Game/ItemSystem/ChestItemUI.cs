using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ChestItemUI : ItemInstanceUI
{
    public Vector3 wearPos = Vector3.up * 2;
    public override List<GameObject> InstantiateNeededItem(IItemEquipper itemEquipper = null)
    {
        if (itemEquipper == null)
        {
            itemEquipper = Z.Player;
        }
        GameObject insOBj = Instantiate(data.pf, Vector3.zero, Quaternion.identity, itemEquipper.GetChest());
        insOBj.transform.localPosition = wearPos;
        List<GameObject> InsObjects = new List<GameObject>();
        InsObjects.Add(insOBj);
        return InsObjects;
    }
}
