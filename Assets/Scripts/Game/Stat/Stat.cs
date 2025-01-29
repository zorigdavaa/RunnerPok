using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public class Stat
{
    private float BaseValue;
    private float FinalValue;
    private List<float> Modifiers = new List<float>();
    private bool isDirty = true; // Flag to track if recalculation is needed

    public Stat(float baseValue)
    {
        BaseValue = baseValue;
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
        BaseValue = value;
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
        Modifiers.ForEach(m => FinalValue += m);
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

    public override string ToString()
    {
        return GetValue().ToString();
    }
}
