using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseStat
{
    public float BaseValue;

    public virtual void SetValue(float value)
    {
        BaseValue = value;
    }
    public virtual void AddValue(float amount)
    {
        BaseValue += amount;
    }
    public virtual float GetValue()
    {
        return BaseValue;
    }
}
