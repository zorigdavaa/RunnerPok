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
        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken first = pool.Get();
        first.transform.position += Vector3.right * 1.5f;
        Shuriken second = pool.Get();
        second.transform.position -= Vector3.right * 1.5f;
        Shuriken third = pool.Get();
        third.transform.position += Vector3.right * 0.5f;
        Shuriken fouth = pool.Get();
        fouth.transform.position -= Vector3.right * 0.5f;
        // third.SideMovement = 2f;
    }
}
