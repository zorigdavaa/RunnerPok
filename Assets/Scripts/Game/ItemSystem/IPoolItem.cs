using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public interface IPoolItem<T> where T : class
{
    public ObjectPool<T> Pool { get; set; }
    public abstract void GetFrompool();
    public abstract void GotoPool();
}
