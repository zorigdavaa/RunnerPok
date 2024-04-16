using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Shuriken : MonoBehaviour
{
    ObjectPool<Shuriken> Pool;

    internal void GetFrompool()
    {
        gameObject.SetActive(true);
        // transform.position = Vector3.zero;
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(5);
            Pool.Release(this);

        }
    }

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
}
