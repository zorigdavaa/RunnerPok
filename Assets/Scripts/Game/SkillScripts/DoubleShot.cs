using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

// [CreateAssetMenu(fileName = "DoubleShot", menuName = "Skill/DoubleShot")]
public class DoubleShot : BaseSkill
{

    public override void Equip()
    {
        Z.Player.OnShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, Shuriken e)
    {
        Use(sender, e);
    }

    public override void Use(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        first.transform.position += Vector3.right * 0.5f;
        first.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.right * 0.1f);
        Shuriken second = pool.Get();
        second.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.left * 0.1f);
        // first.SideMovement = 0.2f;
        // second.SideMovement = -first.SideMovement;
        second.transform.position -= Vector3.right * 0.5f;
    }
}
