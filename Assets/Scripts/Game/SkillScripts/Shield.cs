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
    public override void Equip()
    {
        Z.Player.OnBeforeDamdageTaken += OnBeforeDamdageTaken;
        OnDestroyEvent += Use;
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

    public override void UnEquip()
    {
        Z.Player.RemoveSkill(this);
        Destroy(gameObject);
    }
}
