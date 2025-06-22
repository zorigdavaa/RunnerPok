using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatInt : BaseStat<int>
{
    public int AdditionalValue = 0;
    public List<int> Modifiers = new List<int>();
    public virtual void AddModifier(int value)
    {
        if (value != 0)
        {
            Modifiers.Add(value);
            AdditionalValue += value;
        }
    }

    public virtual void RemoveModifier(int value)
    {
        if (value != 0 && Modifiers.Contains(value))
        {
            Modifiers.Remove(value);
            AdditionalValue -= value;
        }
    }
    public override int GetValue()
    {
        return BaseValue + AdditionalValue;
    }
}
