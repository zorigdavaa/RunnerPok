using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

[CreateAssetMenu(fileName = "DoubleShot", menuName = "Skill/DoubleShot")]
public class DoubleShot : BaseSkill
{

    public override void Equip()
    {
        Z.Player.OnShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, Shuriken e)
    {
        Logic(sender, e);
    }

    public override void Logic(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        first.transform.position += Vector3.right * 0.5f;
        Shuriken second = pool.Get();
        first.SideMovement = 0.2f;
        second.SideMovement = -first.SideMovement;
        second.transform.position -= Vector3.right * 0.5f;
    }
}
