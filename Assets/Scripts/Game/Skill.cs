using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

[CreateAssetMenu(fileName = "Skill", menuName = "Skill/Skill1")]
public class Skill : ScriptableObject
{
    public Sprite Sprite;
    [TextArea] public String Text;
    public Skill nextLevel;

    public void Equip()
    {
        Z.Player.OnShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, Shuriken e)
    {
        Logic(sender, e);
    }

    public virtual void Logic(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        first.transform.position += Vector3.right * 0.5f;
        Shuriken second = pool.Get();
        second.SideMovement = -first.SideMovement;
        second.transform.position -= Vector3.right * 0.5f;
    }

    public void UnEquip()
    {
        throw new NotImplementedException();
    }
}
