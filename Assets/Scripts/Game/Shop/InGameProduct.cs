using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class InGameProduct
{
    public int Price;
    public int Amount;
    public string Name;
    public Sprite Sprite;
    public UnityEvent Logic;
}
