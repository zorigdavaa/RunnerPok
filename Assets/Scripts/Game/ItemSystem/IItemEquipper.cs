using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEquipper
{
    public void EquipItem(ItemInstanceUI item);
    public void UnequipItem(ItemInstanceUI item);

    public ItemInstanceUI GetEquippedItem(WhereSlot slot);
    // public Transform GetRightFoot();
    // public Transform GetLeftFoot();
    // public Transform GetHeadTransform();
    // public Transform GetChest();
    public Transform GetNeededTransform(WhereSlot where);
}