using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public abstract class Character : Mb, IHealth
{
    public LeaderBoardData data;
    private float health;
    public float Health
    {
        get { return health; }
        set
        {
            health = value;
            healthBar.FillHealthBar(Health / MaxHealth);
        }
    }
    public Inventory inventory;

    [SerializeField] float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }


    public bool IsAlive => Health > 0;

    [SerializeField] UIBar healthBar;

    public virtual void TakeDamage(float amount)
    {
        Health += (int)amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // animationController.Die();
        gameObject.layer = 2;
        // Movement.Cancel();
        rb.isKinematic = true;
        healthBar.gameObject.SetActive(false);
    }
    public virtual void AttackProjectile()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ICollisionAction>() != null)
        {
            other.gameObject.GetComponent<ICollisionAction>().CollisionAction(this);
        }
    }
}