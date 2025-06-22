using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

// [CreateAssetMenu(fileName = "StatAdd", menuName = "Skill/StatAdd")]
public class StatAdder : BaseSkill
{
    public int AddDamage = 1;
    public float AddSpeed = 1;
    // public float AddRange = 1;//calculated in second
    public float AddHealth = 1;
    public float Armor = 1;
    public float AddProj = 0;
    public override void OnEquipped()
    {
        base.OnEquipped();
        // Z.Player.AddToSkill(this);
        if (AddHealth != 0)
        {
            Z.Player.Stats.Health.AddToMax(AddHealth);
        }
        if (AddSpeed != 0)
        {
            Z.Player.Stats.AttackSpeed.AddModifier(AddSpeed);
        }
        // if (AddRange != 0)
        // {
        //     Z.Player.AddModifier(AddHealth);
        // }
        if (AddDamage != 0)
        {
            Z.Player.Stats.BaseDamage.AddModifier(AddDamage);
        }
        if (Armor != 0)
        {
            Z.Player.Stats.Armor.AddModifier(Armor);
        }
        if (AddProj != 0)
        {
            Z.Player.Stats.Armor.AddModifier(AddProj);
        }
    }

    public override void Use(object sender, object e)
    {

    }

    public override void OnUnEquip()
    {
        // Z.Player.AddToSkill(this);
        if (AddHealth != 0)
        {
            Z.Player.Stats.Health.AddToMax(-AddHealth);
        }
        if (AddSpeed != 0)
        {
            Z.Player.Stats.AttackSpeed.RemoveModifier(AddSpeed);
        }
        // if (AddRange != 0)
        // {
        //     Z.Player.AddModifier(AddHealth);
        // }
        if (AddDamage != 0)
        {
            Z.Player.Stats.BaseDamage.RemoveModifier(AddDamage);
        }
        if (Armor != 0)
        {
            Z.Player.Stats.Armor.RemoveModifier(Armor);
        }
        if (AddProj != 0)
        {
            Z.Player.Stats.Armor.RemoveModifier(AddProj);
        }
        Z.Player.RemoveSkill(this);
    }
}
