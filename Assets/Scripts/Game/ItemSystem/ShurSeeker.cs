using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ShurSeeker : Shuriken
{
    Transform Target;
    // Start is called before the first frame update
    void Start()
    {
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
                    enemyScript.TakeDamage(data.damageData);
                }
            }
        }
        else
        if (Z.Player.GetState() == PlayerState.Fight)
        {

            transform.localPosition += Vector3.forward * speed * Time.deltaTime;
            FindTarget();
        }
        else
        {
            transform.localPosition += transform.forward * speed * 3 * Time.deltaTime;
            // Pool.Release(this);
        }
        // if (RightAcc < Mathf.Abs(SideMovement * 2))
        // {
        //     RightAcc += Time.deltaTime;
        // }
        // transform.localPosition += Vector3.right * SideMovement * RightAcc * Time.deltaTime;
    }
    float FindDistance = 5;
    public void FindTarget()
    {
        Collider[] AroundObs = Physics.OverlapSphere(transform.position, FindDistance, LayerMask.GetMask("Bot"));
        if (AroundObs.Length > 0)
        {
            float nearDistance = 1000;
            foreach (var item in AroundObs)
            {
                float itemDistance = Vector3.Distance(transform.position, item.transform.position);
                if (itemDistance < nearDistance)
                {
                    Enemy enemyScript = item.GetComponent<Enemy>();
                    if (enemyScript && enemyScript.IsAlive)
                    {

                        Target = item.transform;
                        nearDistance = itemDistance;
                    }
                }
            }

        }
    }
    public override void GotoPool()
    {
        base.GotoPool();
        Target = null;
    }
}
