using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

[CreateAssetMenu(fileName = "ChainLightning", menuName = "Skill/ChainLightning")]
public class ChainLightning : BaseSkill
{
    public GameObject Pf;
    FunctionUpdater updater;
    public override void Equip()
    {
        Z.Player.AddToSkill(this);
        ICastAble insOjb = Instantiate(Pf.gameObject, Z.Player.transform.position, Quaternion.identity, Z.Player.transform).GetComponent<ICastAble>();
        updater = FunctionUpdater.Create(() => { Logic(this, null); insOjb.Cast(); }, 3, name);
        // insOjb
    }

    public override void Logic(object sender, object e)
    {
        // Cast()
        Debug.Log("Lightning");
    }

    public override void UnEquip()
    {
        Z.Player.RemoveSkill(this);
        // updater.Remove();
        FunctionUpdater.StopTimer(name);
        // FunctionUpdater.RemoveTimer()
    }
}
