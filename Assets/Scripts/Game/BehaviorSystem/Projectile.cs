using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class Projectile : MonoBehaviour
{
    public float speed = 10;
    // public float Damage = 5;
    public DamageData damageData;
    // Start is called before the first frame update
    void Start()
    {
        // Damage *= Z.LS.LastInstLvl.DamageMultiplier;
        damageData.damage *= Z.LS.LastInstLvl.DamageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0, 360 * Time.deltaTime, 0);
        ProjMove();
    }

    public virtual void ProjMove()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.blue, 1);
        // Debug.DrawLine(transform.position, transform.localPosition + transform.forward, Color.black, 1);
        // transform.localPosition -= transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            // other.GetComponent<Player>().TakeDamage(-Damage);
            other.GetComponent<Player>().TakeDamage(damageData);
            Destroy(gameObject);
        }
    }
}