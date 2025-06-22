using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class BaseStat<T>
{
    public T BaseValue;

    public virtual void SetValue(T value)
    {
        BaseValue = value;
    }
    public virtual void AddValue(T amount)
    {
        if (typeof(T) == typeof(float) && amount is float f)
        {
            float result = (float)(object)BaseValue + f;
            BaseValue = (T)(object)result;
        }
        else
        {
            throw new System.NotImplementedException("AddValue not implemented for BaseStat");
        }
        // BaseValue += amount;
    }
    public virtual T GetValue()
    {
        return BaseValue;
    }
}
