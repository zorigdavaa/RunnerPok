using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public Transform transform { get; }
    public GameObject gameObject { get; }
    public float MaxHealth { get; set; }
    public float Health { get; set; }
    public bool IsAlive { get; }
    // public void TakeDamage(DamageData amount);
    public void TakeDamage(float amount);
    public void Die();
}
