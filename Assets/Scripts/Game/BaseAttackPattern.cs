using System;
using System.Collections;
using UnityEngine;
public abstract class BaseAttackPattern : ScriptableObject
{
    public abstract void AttackProjectile(Animal animal);
    public abstract IEnumerator Pattern(Animal animal);

    public abstract float GetCoolDown();
}