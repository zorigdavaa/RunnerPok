using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

public class BaseSkill : ScriptableObject
{
    public Sprite Sprite;
    [TextArea] public string Text;
    public BaseSkill nextLevel;

    public virtual void Equip()
    {

    }

    public virtual void Logic(object sender, object e)
    {
        // Shuriken first = (Shuriken)e;
        // var pool = (ObjectPool<Shuriken>)sender;
        // first.transform.position += Vector3.right * 0.5f;
        // Shuriken second = pool.Get();
        // second.SideMovement = -first.SideMovement;
        // second.transform.position -= Vector3.right * 0.5f;
    }

    public virtual void UnEquip()
    {
        throw new NotImplementedException();
    }
}
