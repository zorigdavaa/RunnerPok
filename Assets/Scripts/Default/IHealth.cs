using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public Stat MaxHealth { get; set; }
    public Stat Health { get; set; }
    public bool IsAlive { get; }
    // public void TakeDamage(DamageData amount);
    public void TakeDamage(float amount);
    public void TakeDamage(DamageData data);
    public void Die();
}
