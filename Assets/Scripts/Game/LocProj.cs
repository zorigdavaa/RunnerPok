using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class LocProj : Projectile
{
    public Transform Target;
    public float rotSpeed = 0.05f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target)
        {
            ProjMove();
        }
        // else
        // {
        //     Destroy(gameObject);
        // }
    }
    public override void ProjMove()
    {
        Vector3 sameYPos = new Vector3(Target.position.x, transform.position.y, Target.position.z);
        Vector3 dir = sameYPos - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotSpeed);
        base.ProjMove();
    }
}
