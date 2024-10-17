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
        Health -= (int)finalDamage;
        healthBar.OnTop();
        if (Health <= 0)
        {
            Die();
        }
    }
}
