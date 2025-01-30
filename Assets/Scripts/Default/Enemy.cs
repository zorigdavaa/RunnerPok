using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public abstract class Enemy : Character
{
    public override void TakeDamage(DamageData data)
    {
        float multiploer = data.Type.GetEffectiveMultiplier(_Element);
        float finalDamage = data.damage * multiploer;
        Stats.Health -= (int)finalDamage;
        healthBar.OnTop();
        if (Stats.Health.GetValue() <= 0)
        {
            Die();
        }
    }
}
