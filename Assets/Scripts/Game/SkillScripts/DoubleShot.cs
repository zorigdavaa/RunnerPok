using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

// [CreateAssetMenu(fileName = "DoubleShot", menuName = "Skill/DoubleShot")]
public class DoubleShot : BaseSkill
{

    public override void OnEquipped()
    {
        Z.Player.OnShoot += OnPlayerShoot;
    }

    private void OnPlayerShoot(object sender, List<Shuriken> e)
    {
        Use(sender, e[0]);
    }

    public override void Use(object sender, object e)
    {

        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken first = pool.Get();
        first.transform.position += Vector3.right * 1.5f;
        Shuriken second = pool.Get();
        second.transform.position -= Vector3.right * 1.5f;
    }
}
