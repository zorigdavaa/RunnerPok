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
public class Shuriken : MonoBehaviour, ISaveAble
{
    protected ObjectPool<Shuriken> Pool;
    protected Coroutine AutoGotoPoolCor;
    [SerializeField] protected ItemData data;
    protected int level = 1;
    // int Damage = 5;
    protected DamageData damageData;
    protected Transform Graphics;

    internal void GetFrompool()
    {
        gameObject.SetActive(true);
        // transform.position = Vector3.zero;
        AutoGotoPoolCor = StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(5);
            Pool.Release(this);
            AutoGotoPoolCor = null;

        }
    }
    //Should Only called from Release
    internal void GotoPool()
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

    internal void SetPool(ObjectPool<Shuriken> pool)
    {
        Pool = pool;
    }

    // Start is called before the first frame update
    protected void Start()
    {
        RetrieveData();
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
            RightAcc = 0;
            enemy.TakeDamage(data.damageData);
            Pool.Release(this);
            if (AutoGotoPoolCor != null)
            {
                StopCoroutine(AutoGotoPoolCor);
                AutoGotoPoolCor = null;
            }
        }
    }

    public void SaveData()
    {
        // PlayerPrefs.SetInt(data.name, level);
    }

    public void RetrieveData()
    {
        level = PlayerPrefs.GetInt(data.name, level);
        damageData.damage = data.BaseDamage + data.AddDamage[level];
        damageData.Type = data.damageData.Type;
    }
}
