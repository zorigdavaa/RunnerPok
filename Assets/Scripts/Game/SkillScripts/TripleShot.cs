using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

// [CreateAssetMenu(fileName = "TripleShot", menuName = "Skill/TripleShot")]
public class TripleShot : DoubleShot
{
    public override void Use(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken second = pool.Get();
        Shuriken third = pool.Get();
        first.transform.rotation = Quaternion.identity;
        // first.SideMovement = 0f;
        // second.SideMovement = -2f;
        second.transform.position -= Vector3.right * 0.5f;
        third.transform.position += Vector3.right * 0.5f;
        second.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.right * 0.5f);
        third.transform.rotation = Quaternion.LookRotation(Z.Player.transform.forward + Vector3.left * 0.5f);
        // third.SideMovement = 2f;
    }
}
