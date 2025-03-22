using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using ZPackage;

//IDea 
//BounceShuriken
//PiercingShuriken
//PoisonShuriken
//BombShuriken
//LightningShuriken
//SeekingShuriken
public class Shuriken : BaseEquipedItem, IPoolItem<Shuriken>
{
    public ObjectPool<Shuriken> Pool { get; set; }
    protected Coroutine AutoGotoPoolCor;
    protected Transform Graphics;



    public void GetFrompool()
    {
        gameObject.SetActive(true);
        transform.rotation = Quaternion.identity;
        // transform.position = Vector3.zero;
        AutoGotoPoolCor = StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(data.BaseRange);
            Pool.Release(this);
            AutoGotoPoolCor = null;

        }
    }
    //Should Only called from Release
    public virtual void GotoPool()
    {
        gameObject.SetActive(false);
        transform.position = Vector3.zero;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Graphics = transform.GetChild(0);
        // Z.Player.OnStateChanged += OnPlayerStateChange;
    }

    protected float speed = 15;
    // Update is called once per frame
    void Update()
    {
        ShurikenBehavior();
    }

    public virtual void ShurikenBehavior()
    {
        Graphics.Rotate(0, 360 * Time.deltaTime, 0);
        if (Z.Player.GetState() == PlayerState.Fight)
        {

            transform.localPosition += transform.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += transform.forward * speed * 3 * Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        Impact(enemy);
    }

    public virtual void Impact(Enemy enemy)
    {
        if (enemy && enemy.IsAlive)
        {
            // enemy.TakeDamage(data.damageData);
            enemy.TakeDamage(DamageData);

            Pool.Release(this);
            if (AutoGotoPoolCor != null)
            {
                StopCoroutine(AutoGotoPoolCor);
                AutoGotoPoolCor = null;
            }
        }
    }

    public virtual void OnShoot(Player player)
    {

    }
}
