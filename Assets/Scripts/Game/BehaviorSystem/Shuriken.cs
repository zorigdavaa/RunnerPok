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

    // int Damage = 5;
    // protected DamageData damageData;
    protected Transform Graphics;



    public void GetFrompool()
    {
        gameObject.SetActive(true);
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
        SideMovement = 0;
        RightAcc = 0;
        // StartCoroutine(LocalCoroutine());
        // IEnumerator LocalCoroutine()
        // {
        //     yield return new WaitForSeconds(wait);

        // }
    }

    // Start is called before the first frame update
    protected void Start()
    {
        Graphics = transform.GetChild(0);
        // Z.Player.OnStateChanged += OnPlayerStateChange;
    }


    public float SideMovement;
    protected float RightAcc = 0;
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

            transform.localPosition += Vector3.forward * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition += Vector3.forward * speed * 3 * Time.deltaTime;
            // Pool.Release(this);
        }
        // transform.localPosition += Vector3.right * SideMovement * Time.deltaTime;
        if (RightAcc < Mathf.Abs(SideMovement * 2))
        {
            RightAcc += Time.deltaTime;
        }
        transform.localPosition += Vector3.right * SideMovement * RightAcc * Time.deltaTime;
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
}
