using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "IAP - ", menuName = "IAP/IAP Product", order = 1)]
public class CkProduct : ScriptableObject
{
    [SerializeField] string id;
    [SerializeField] ProductType productType;
    public bool IsIAP;
    public string ID => id;
    public ProductType ProductType => productType;
    public UnityEvent Logic;
}
