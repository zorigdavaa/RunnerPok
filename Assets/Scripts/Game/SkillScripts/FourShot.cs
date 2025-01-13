using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "FourShot", menuName = "Skill/FourShot")]
public class FourShot : TripleShot
{
    public override void Logic(object sender, object e)
    {
        Shuriken first = (Shuriken)e;
        var pool = (ObjectPool<Shuriken>)sender;
        Shuriken second = pool.Get();
        Shuriken third = pool.Get();
        Shuriken Fourth = pool.Get();
        first.SideMovement = -2f;
        second.SideMovement = -0.5f;
        third.SideMovement = 2f;
        Fourth.SideMovement = 0.5f;
    }
}
