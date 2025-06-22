using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Shield : BaseSkill
{
    public int Count = 5;

    public Action<object, object> OnDestroyEvent { get; internal set; }

    // Start is called before the first frame update
    public override void OnEquipped()
    {
        Z.Player.OnBeforeDamdageTaken += OnBeforeDamdageTaken;
        OnDestroyEvent += Use;
    }
    public override void Use(object sender, object e)
    {
        Z.Player.RemoveSkill(this);
    }

    private void OnBeforeDamdageTaken(ref float damage)
    {
        damage = 0;
        DecreaseCount();
    }

    public void DecreaseCount()
    {
        Count--;
        Debug.Log("Decreased");
        if (Count < 0)
        {
            Z.Player.OnBeforeDamdageTaken -= OnBeforeDamdageTaken;
            OnDestroyEvent?.Invoke(gameObject, null);
        }
    }

    public override void OnUnEquip()
    {

    }
}
