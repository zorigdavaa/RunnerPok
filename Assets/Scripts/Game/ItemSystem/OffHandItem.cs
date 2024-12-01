using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[CreateAssetMenu(fileName = "OffHandItem", menuName = "Inventory/OffHand")]
public class OffHandItem : ItemData
{
    public const float Delay = 3;
    public float Timer = Delay;
    public EventHandler OnOffHandItem;
    // Update is called once per frame
    public void Update()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Timer = Delay;
            OnOffHandItem?.Invoke(this, EventArgs.Empty);
        }
    }
}
