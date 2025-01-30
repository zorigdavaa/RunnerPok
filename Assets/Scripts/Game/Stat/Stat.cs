using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat : BaseStat
{
    public float AdditionalValue = 0;
    public List<float> Modifiers = new List<float>();
    public virtual void AddModifier(float value)
    {
        if (value != 0)
        {
            Modifiers.Add(value);
            AdditionalValue += value;
        }
    }

    public virtual void RemoveModifier(float value)
    {
        if (value != 0 && Modifiers.Contains(value))
        {
            Modifiers.Remove(value);
            AdditionalValue -= value;
        }
    }
    public override float GetValue()
    {
        return BaseValue + AdditionalValue;
    }
}
