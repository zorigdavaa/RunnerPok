using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEquipper
{
    public void EquipItem(BaseItemUI item);
    public void UnequipItem(BaseItemUI item);

    public BaseItemUI GetEquippedItem(WhereSlot slot);
    public Transform GetRightFoot();
    public Transform GetLeftFoot();
    public Transform GetHeadTransform();
    public Transform GetChest();
    public void NewMethod(BaseItemUI value)
    {
        Debug.Log("Test");
    }
}