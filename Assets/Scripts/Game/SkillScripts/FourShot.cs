using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

// [CreateAssetMenu(fileName = "FourShot", menuName = "Skill/FourShot")]
public class FourShot : TripleShot
{
    public override void Use(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken second = pool.Get();
        Shuriken third = pool.Get();
        Shuriken Fourth = pool.Get();
        first.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.left * 0.1f);
        second.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.right * 0.1f);
        third.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.left * 0.3f);
        Fourth.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.right * 0.3f);
        // first.SideMovement = -2f;
        // second.SideMovement = -0.5f;
        // third.SideMovement = 2f;
        // Fourth.SideMovement = 0.5f;
    }
}
