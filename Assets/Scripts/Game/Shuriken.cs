using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shuriken : MonoBehaviour
{
    ObjectPool<Shuriken> Pool;
    Coroutine AutoGotoPoolCor;
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

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 360 * Time.deltaTime, 0);
        transform.localPosition += Vector3.forward * 8 * Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy && enemy.IsAlive)
        {
            enemy.TakeDamage(-2);
            Pool.Release(this);
            if (AutoGotoPoolCor != null)
            {
                StopCoroutine(AutoGotoPoolCor);
                AutoGotoPoolCor = null;
            }
        }
    }
}
