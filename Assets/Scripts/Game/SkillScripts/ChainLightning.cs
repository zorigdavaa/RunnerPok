using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

[CreateAssetMenu(fileName = "ChainLightning", menuName = "Skill/ChainLightning")]
public class ChainLightning : BaseSkill
{
    public ChainLightObject Pf;
    FunctionUpdater updater;
    public override void Equip()
    {
        Z.Player.AddToSkill(this);
        ChainLightObject insOjb = Instantiate(Pf, Z.Player.transform.position, Quaternion.identity, Z.Player.transform);
        updater = FunctionUpdater.Create(() => { Logic(this, null); insOjb.Cast(); }, 3, name);
        // insOjb
    }

    public override void Logic(object sender, object e)
    {
        // Cast()
        Debug.Log("Lightning");
        // Shuriken first = (Shuriken)e;
        // var pool = (ObjectPool<Shuriken>)sender;
        // first.transform.position += Vector3.right * 0.5f;
        // Shuriken second = pool.Get();
        // second.SideMovement = -first.SideMovement;
        // second.transform.position -= Vector3.right * 0.5f;
    }

    public override void UnEquip()
    {
        Z.Player.RemoveSkill(this);
        // updater.Remove();
        FunctionUpdater.StopTimer(name);
        // FunctionUpdater.RemoveTimer()
    }
}
