using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipedItem : MonoBehaviour
{
    public ItemInstance ItemInstance;
    public DamageData DamageData;
    [SerializeField] protected ItemData data;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    internal void SetInstance(ItemInstance handItem)
    {
        ItemInstance = handItem;
        DamageData = new DamageData(data.Element, handItem.GetDamdage());
    }
}