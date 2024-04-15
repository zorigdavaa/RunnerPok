using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using ZPackage;

public abstract class Character : Mb, IHealth
{
    public MovementForgeRun movement;
    public LeaderBoardData data;
    private int health;
    public int Health
    {
        get { return health; }
        set
        {
            health = value;
            healthBar.fillAmount = value * 0.01f;
        }
    }

    public AnimationController animationController;
    public Inventory inventory;

    int IHealth.Health { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField] Image healthBar;

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
        animationController.Die();
        gameObject.layer = 2;
        movement.Cancel();
        rb.isKinematic = true;
        healthBar.transform.parent.gameObject.SetActive(false);
    }
}