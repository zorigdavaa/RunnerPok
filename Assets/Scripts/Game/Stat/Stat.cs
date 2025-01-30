using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stat
{
    [SerializeField]
    private float BaseValue;
    [SerializeField]
    private float FinalValue;
    [SerializeField] float MaxValue = 100;
    [SerializeField] private List<float> Modifiers = new List<float>();
    [SerializeField] private bool isDirty = true; // Flag to track if recalculation is needed

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
        MaxValue = baseValue;
        RecalculateFinalValue(); // Initialize FinalValue properly
    }

    // GetValue only recalculates if needed
    public float GetValue()
    {
        if (isDirty)
        {
            RecalculateFinalValue();
        }
        return FinalValue;
    }

    // Updates BaseValue and marks value as needing recalculation
    public void SetBaseValue(float value)
    {
        BaseValue = Mathf.Clamp(value, 0, MaxValue);
        isDirty = true; // Mark dirty
    }

    public void AddModifier(float value)
    {
        if (value != 0)
        {
            Modifiers.Add(value);
            isDirty = true; // Mark dirty
        }
    }

    public void RemoveModifier(float value)
    {
        if (value != 0 && Modifiers.Contains(value))
        {
            Modifiers.Remove(value);
            isDirty = true; // Mark dirty
        }
    }

    // Force recalculation only when needed
    private void RecalculateFinalValue()
    {
        FinalValue = BaseValue;
        Modifiers.ForEach(m => { FinalValue += m; MaxValue += m; });
        isDirty = false; // Reset dirty flag after updating
    }

    // Overloaded operators (optional)
    public static Stat operator -(Stat stat, float amount)
    {
        stat.SetBaseValue(stat.BaseValue - amount);
        return stat;
    }

    public static Stat operator +(Stat stat, float amount)
    {
        stat.SetBaseValue(stat.BaseValue + amount);
        return stat;
    }
    public void AddToMax(float value)
    {
        MaxValue += value;
        SetBaseValue(BaseValue + value);
    }

    public override string ToString()
    {
        return GetValue().ToString();
    }

    internal float GetMax()
    {
        return MaxValue;
    }

    internal float GetPercent()
    {
        return BaseValue / MaxValue;
    }
}
