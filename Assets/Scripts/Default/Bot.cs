using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Pathfinding;

public class Bot : Enemy
{
    public Transform Target;
    [SerializeField] Transform Chest;
    public bool UseAI = false;
    public Projectile ProjectilePf;
    private void Start()
    {
        // Target = FindObjectOfType<Player>().transform;
        // movement.GoToPosition(Target);
        // animationController.Set8WayLayerWeight(false);
    }
    float attackTimer = 3;
    private void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0)
        {
            attackTimer = 3;
            AttackProjectile();
        }
    }

    public void GotoTarget()
    {
        movement.GoToPosition(Target);
    }
    public void GotoPos(Vector3 pos)
    {
        movement.GoToPosition(pos);
    }
    public void GotoPath(List<Vector3> path)
    {
        // movement.GotoPath(path);
    }

    public override void Die()
    {
        base.Die();
        // rb.isKinematic = true;
    }
    public override void AttackProjectile()
    {
        Instantiate(ProjectilePf, transform.position + Vector3.up, transform.rotation, transform.parent);
    }
}
