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
    // private float health;
    // public float Health
    // {
    //     get { return health; }
    //     set
    //     {
    //         health = value;
    //         healthBar.FillHealthBar(Health / MaxHealth);
    //     }
    // }
    public CharacterStats Stats;
    // [SerializeField]
    // private Stat health;
    // public Stat Health
    // {
    //     get { return health; }
    //     set
    //     {
    //         health = value;
    //         healthBar.FillHealthBar(Health.GetValue() / MaxHealth.GetValue());
    //     }
    // }
    public delegate void OnBeforeTakeDamageHandler(ref float damage);
    public OnBeforeTakeDamageHandler OnBeforeDamdageTaken;
    public Inventory inventory;

    // [SerializeField] Stat maxHealth;
    // public Stat MaxHealth
    // {
    //     get { return maxHealth; }
    //     set { maxHealth = value; }
    // }
    // [SerializeField] Stat armor;
    // public Stat Armor
    // {
    //     get { return armor; }
    //     set { armor = value; }
    // }

    public bool IsAlive => Stats.Health.GetValue() > 0;

    public UIBar healthBar;

    public virtual void TakeDamage(float amount)
    {
        OnBeforeDamdageTaken?.Invoke(ref amount);
        Stats.Health += amount;
        // float addedAmount = Health.GetValue() + amount;
        // Health.SetValue(addedAmount);
        if (Stats.Health.GetValue() <= 0)
        {
            Die();
        }
        healthBar.FillHealthBar(Stats.Health.GetValue() / Stats.Health.GetMax());
    }
    public virtual void TakeDamage(DamageData data)
    {
        float multiploer = data.Type.GetEffectiveMultiplier(_Element);
        float finalDamage = data.damage * multiploer;
        float armorReduction = Stats.Armor.GetValue() / (Stats.Armor.GetValue() + 20);
        finalDamage = finalDamage * (1 - armorReduction);
        OnBeforeDamdageTaken?.Invoke(ref finalDamage);
        Stats.Health -= (int)finalDamage;
        if (Stats.Health.GetValue() <= 0)
        {
            Die();
        }
        healthBar.FillHealthBar(Stats.Health.GetValue() / Stats.Health.GetMax());
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
