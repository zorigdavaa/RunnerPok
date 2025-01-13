using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "TripleShot", menuName = "Skill/TripleShot")]
public class TripleShot : DoubleShot
{
    public override void Logic(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken second = pool.Get();
        Shuriken third = pool.Get();
        first.SideMovement = 0f;
        second.SideMovement = -2f;
        second.transform.position -= Vector3.right * 0.5f;
        third.transform.position += Vector3.right * 0.5f;
        third.SideMovement = 2f;
    }
}
