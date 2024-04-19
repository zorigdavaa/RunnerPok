using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{
    public Projectile ProjectilePf;
    public new AnimalAnim animationController;
    private void Start()
    {
        Health = MaxHealth;
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
