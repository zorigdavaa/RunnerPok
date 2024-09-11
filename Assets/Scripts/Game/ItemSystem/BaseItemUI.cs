using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseItemUI : MonoBehaviour, IUpgradeAble
{
    public ItemData data;
    public virtual void Upgrade()
    {

    }
}
