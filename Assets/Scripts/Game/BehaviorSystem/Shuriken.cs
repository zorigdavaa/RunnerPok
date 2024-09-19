using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shuriken : MonoBehaviour, ISaveAble
{
    ObjectPool<Shuriken> Pool;
    Coroutine AutoGotoPoolCor;
    [SerializeField] ItemData data;
    int level = 1;
    // int Damage = 5;
    DamageData damageData;
    Transform Graphics;

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
    void Start()
    {
        RetrieveData();
        Graphics = transform.GetChild(0);
    }
    public float SideMovement;
    float RightAcc = 0;
    // Update is called once per frame
    void Update()
    {
        Graphics.Rotate(0, 360 * Time.deltaTime, 0);
        transform.localPosition += Vector3.forward * 15 * Time.deltaTime;
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
        RightAcc = 0;
        if (enemy && enemy.IsAlive)
        {
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
