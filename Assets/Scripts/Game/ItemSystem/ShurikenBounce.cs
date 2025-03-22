using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZPackage;


public class ShurikenBounce : Shuriken
{
    protected Transform Target;
    public int BounceAmount = 3;
    DamageData damageDataCopy;
    List<Enemy> impactedEnemy = new List<Enemy>();
    public float FindDistance = 5;
    // Start is called before the first frame update
    protected override void Start()
    {
        // firstImpacted = false;
        damageDataCopy = DamageData;
        base.Start();
    }

    public override void ShurikenBehavior()
    {
        Graphics.Rotate(0, 360 * Time.deltaTime, 0);
        if (Target)
        {
            transform.position = Vector3.MoveTowards(transform.position, Target.position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, Target.position) < 0.2f)
            {
                Enemy enemyScript = Target.GetComponent<Enemy>();
                if (enemyScript)
                {
                    // enemyScript.TakeDamage(data.damageData);
                    damageDataCopy.damage *= 0.5f;
                    enemyScript.TakeDamage(damageDataCopy);
                    impactedEnemy.Add(enemyScript);
                }
                FindTarget();
            }
        }
        else
        if (Z.Player.GetState() == PlayerState.Fight)
        {

            transform.localPosition += Vector3.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += Vector3.forward * speed * 3 * Time.deltaTime;
            // Pool.Release(this);
        }
        // if (RightAcc < Mathf.Abs(SideMovement * 2))
        // {
        //     RightAcc += Time.deltaTime;
        // }
        // transform.localPosition += Vector3.right * SideMovement * RightAcc * Time.deltaTime;
    }

    public void FindTarget()
    {
        Collider[] AroundObs = Physics.OverlapSphere(transform.position, FindDistance, LayerMask.GetMask("Bot"));
        bool FoundNewOne = false;
        if (AroundObs.Length > 0)
        {
            float nearDistance = 1000;
            foreach (var item in AroundObs)
            {
                float itemDistance = Vector3.Distance(transform.position, item.transform.position);
                if (itemDistance < nearDistance)
                {
                    Enemy enemyScript = item.GetComponent<Enemy>();
                    if (enemyScript && !impactedEnemy.Contains(enemyScript))
                    {
                        Target = item.transform;
                        nearDistance = itemDistance;
                        FoundNewOne = true;
                    }
                }
            }
            if (!FoundNewOne)
            {
                Pool.Release(this);
            }
        }
        else
        {
            Pool.Release(this);
        }

    }
    public override void GotoPool()
    {
        base.GotoPool();
        BounceAmount = 3;
        // damageDataCopy = data.damageData;
        damageDataCopy = DamageData;
        GetComponent<Collider>().enabled = true;
        impactedEnemy.Clear();
        Target = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy && enemy.IsAlive)
        {
            if (!impactedEnemy.Any())
            {
                Impact(enemy);
                GetComponent<Collider>().enabled = false;
            }
        }
    }
    public override void Impact(Enemy enemy)
    {
        if (enemy && enemy.IsAlive)
        {
            BounceAmount--;
            impactedEnemy.Add(enemy);
            // enemy.TakeDamage(data.damageData);
            enemy.TakeDamage(damageDataCopy);
            // RightAcc = 0;
            if (BounceAmount < 1)
            {
                Pool.Release(this);
            }
            else
            {
                FindTarget();
            }
            if (AutoGotoPoolCor != null)
            {
                StopCoroutine(AutoGotoPoolCor);
                AutoGotoPoolCor = null;
            }
        }
    }
}
