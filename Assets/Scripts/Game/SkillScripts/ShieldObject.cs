using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ShieldObject : MonoBehaviour
{
    public int Count = 5;

    public Action<object, object> OnDestroyEvent { get; internal set; }

    // Start is called before the first frame update
    void Start()
    {
        Z.Player.OnBeforeDamdageTaken += OnBeforeDamdageTaken;
    }

    private void OnBeforeDamdageTaken(ref float damage)
    {
        damage = 0;
        DecreaseCount();
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
        Debug.Log("Decreased");
        if (Count < 0)
        {
            Z.Player.OnBeforeDamdageTaken -= OnBeforeDamdageTaken;
            OnDestroyEvent?.Invoke(gameObject, null);
        }
    }
}
