using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemUI : MonoBehaviour, IUpgradeAble
{
    public ItemData data;
    public UISlot currentSlot; 
    public virtual void Upgrade()
    {

    }
    public virtual void ShowInfo()
    {
        
    }
}
