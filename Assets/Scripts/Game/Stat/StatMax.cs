using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatMax : BaseStat
{
    [SerializeField] float MaxValue = 100;
    public StatMax(float baseValue)
    {
        BaseValue = baseValue;
        MaxValue = baseValue;
    }

    // Updates BaseValue and marks value as needing recalculation
    public override void SetValue(float value)
    {
        BaseValue = Mathf.Clamp(value, 0, MaxValue);
    }
    public void AddToMax(float value)
    {
        MaxValue += value;
        if (value > 0)
        {
            SetValue(BaseValue + value);
        }
        else
        {
            SetValue(BaseValue);
        }
    }
    public override void AddValue(float amount)
    {
        SetValue(BaseValue + amount);
    }

    public float GetMax()
    {
        return MaxValue;
    }

    public float GetPercent()
    {
        return BaseValue / MaxValue;
    }
}
