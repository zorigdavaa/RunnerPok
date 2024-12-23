using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffHandItemInstance : ItemInstance
{
    public const float Delay = 3;
    public float Timer = Delay;
    public EventHandler OnOffHandItem;
    public void Tick()
    {
        Timer -= Time.deltaTime;
        if (Timer < 0)
        {
            Timer = Delay;
            OnOffHandItem?.Invoke(this, EventArgs.Empty);
        }
    }
}
