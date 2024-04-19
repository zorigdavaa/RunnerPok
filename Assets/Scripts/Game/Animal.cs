using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Enemy
{
    public List<GameObject> ProjectilePfs;
    public AnimalAnim animationController;
    public MovementForgeRun movement;
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
        animationController.Die();
        movement.Cancel();
        // rb.isKinematic = true;
    }
    public override void AttackProjectile()
    {
        GameObject pf = ProjectilePfs[Random.Range(0, ProjectilePfs.Count)];
        GameObject inSob = Instantiate(pf, transform.position + Vector3.up, transform.rotation, transform.parent);
        Destroy(inSob, 10);
    }
}
