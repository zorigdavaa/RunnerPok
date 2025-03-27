using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public class ItemInstanceUI : BaseItemUI
{
    public virtual void EquipItem(IItemEquipper character = null)
    {
        if (character == null) character = Z.Player;

        character.EquipItem(this);
    }
    public virtual void UnEquipItem(IItemEquipper Character = null)
    {
        if (Character == null)
        {
            Character = Z.Player;
        }
        Character.UnequipItem(this);
    }
}
