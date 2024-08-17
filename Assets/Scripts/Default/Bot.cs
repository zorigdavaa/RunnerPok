using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bot : Enemy
{
    [SerializeField] AnimationController animationController;
    [SerializeField] MovementForgeRun movement;
    public Transform Target;
    public Projectile ProjectilePf;
    private void Start()
    {
        Health = MaxHealth;
        // Target = FindObjectOfType<Player>().transform;
        // movement.GoToPosition(Target);
        // animationController.Set8WayLayerWeight(false);
    }
    float attackTimer = 3;
    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0 && IsAlive)
        {
            attackTimer = 3;
            AttackProjectile();
        }
    }

    public override void Die()
    {
        base.Die();
        // rb.isKinematic = true;
    }
    public override void AttackProjectile()
    {
        Projectile inSob = Instantiate(ProjectilePf, transform.position + Vector3.up, transform.rotation, transform.parent);
        Destroy(inSob.gameObject, 10);
    }
}
