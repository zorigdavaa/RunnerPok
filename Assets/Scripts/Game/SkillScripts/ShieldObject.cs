using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour
{
    public int Count = 5;

    public Action<object, object> OnDestroyEvent { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // private void OnTriggerEnter(Collider other)
    // {

    //     Projectile proj = other.transform.GetComponent<Projectile>();
    //     if (proj != null)
    //     {
    //         DecreaseCount();
    //     }

    // }

    public void DecreaseCount()
    {
        Count--;
        if (Count < 0)
        {
            OnDestroyEvent?.Invoke(gameObject, null);
        }
    }
}
