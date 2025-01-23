using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using ZPackage;

public abstract class Character : Mb, IHealth
{
    public ElementType _Element;
    // public LeaderBoardData data;
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
    public delegate void OnBeforeTakeDamageHandler(ref float damage);
    public OnBeforeTakeDamageHandler OnBeforeDamdageTaken;
    public Inventory inventory;

    [SerializeField] float maxHealth;
    public float MaxHealth
    {
        get { return maxHealth; }
        set { maxHealth = value; }
    }
    [SerializeField] float armor;
    public float Armor
    {
        get { return armor; }
        set { armor = value; }
    }

    public bool IsAlive => Health > 0;

    public UIBar healthBar;

    public virtual void TakeDamage(float amount)
    {
        OnBeforeDamdageTaken?.Invoke(ref amount);
        Health += (int)amount;
        if (Health <= 0)
        {
            Die();
        }
    }
    public virtual void TakeDamage(DamageData data)
    {
        float multiploer = data.Type.GetEffectiveMultiplier(_Element);
        float finalDamage = data.damage * multiploer;
        float armorReduction = Armor / (Armor + 20);
        finalDamage = finalDamage * (1 - armorReduction);
        OnBeforeDamdageTaken?.Invoke(ref finalDamage);
        Health -= (int)finalDamage;
        if (Health <= 0)
        {
            Die();
        }
    }
    public EventHandler Ondeath;

    public virtual void Die()
    {
        // animationController.Die();
        // Movement.Cancel();
        gameObject.layer = 2;
        rb.isKinematic = true;
        healthBar.gameObject.SetActive(false);
        Ondeath?.Invoke(this, EventArgs.Empty);
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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ICollisionAction>() != null)
        {
            other.gameObject.GetComponent<ICollisionAction>().CollisionAction(this);
        }
    }
}
