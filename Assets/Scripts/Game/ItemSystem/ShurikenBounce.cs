using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ShurikenBounce : Shuriken
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ShurikenBehavior();
    }
    public override void ShurikenBehavior()
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
    public override void Impact(Collider other)
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
}
