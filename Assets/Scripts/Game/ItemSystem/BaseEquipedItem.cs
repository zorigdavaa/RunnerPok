using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEquipedItem : MonoBehaviour
{
    public ItemInstanceUI ItemInstance;
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
    internal void SetInstance(ItemInstanceUI handItem)
    {
        ItemInstance = handItem;
        DamageData = new DamageData(data.Element, handItem.GetDamdage());
    }
    //Use it when there is no itemInstance this will use only baseDamage not lvl upgrade
    internal void SetDamage()
    {
        DamageData = new DamageData(data.Element, data.BaseDamage);
    }
}
