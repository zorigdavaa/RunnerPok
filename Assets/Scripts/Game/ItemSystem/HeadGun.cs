using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

public class HeadGun : BaseEquipedItem
{
    public Shuriken ShurikenPf;
    ObjectPool<Shuriken> Pool;
    // Start is called before the first frame update
    void Start()
    {
        InitHandPool();
    }
    private void InitHandPool()
    {
        Pool = new ObjectPool<Shuriken>(() =>
        {
            Shuriken spear = Instantiate(ShurikenPf, transform.position, Quaternion.identity, Z.Player.transform.parent).GetComponent<Shuriken>();
            spear.Pool = Pool;
            spear.SetInstance(ItemInstance);
            return spear;
            // return new GameObject();
        }, (s) =>
        {
            s.transform.position = transform.position + new Vector3(0, 1, 0);
            s.GetFrompool();
        }, (s) =>
        {
            // release
            s.GotoPool();
        });
    }
    Transform Target;
    float WaitTime = 1;
    float WaitTimeDefault = 1;
    // Update is called once per frame
    void Update()
    {
        if (Z.Player.GetState() == PlayerState.Fight)
        {
            WaitTime -= Time.deltaTime;
            if (WaitTime < 0)
            {
                WaitTime = WaitTimeDefault;
                FindTarget();
                if (Target != null)
                {
                    Shuriken shur = Pool.Get();
                    Vector3 goPos = Target.transform.position + Vector3.up * 1.5f;
                    Vector3 sameYPsos = shur.transform.position;
                    sameYPsos.y = goPos.y;
                    shur.transform.position = sameYPsos;
                    shur.transform.LookAt(Target);
                }
            }
        }
    }
    float FindDistance = 10;
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
}
