using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

[CreateAssetMenu(fileName = "StatAdd", menuName = "Skill/StatAdd")]
public class StatAdder : BaseSkill
{
    public int AddDamage = 1;
    public float AddSpeed = 1;
    public float AddRange = 1;//calculated in second
    public float AddHealth = 1;
    public override void Equip()
    {
        Z.Player.AddToSkill(this);
        if (AddHealth != 0)
        {
            Z.Player.Stats.Health.AddToMax(AddHealth);
        }
    }

    public override void Logic(object sender, object e)
    {

    }

    public override void UnEquip()
    {
        Z.Player.Stats.Health.AddToMax(-AddHealth);
        if (AddHealth != 0)
            Z.Player.RemoveSkill(this);
    }
}
