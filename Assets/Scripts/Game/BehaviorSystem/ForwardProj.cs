using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class ForwardProj : Projectile
{
    public bool DestroyAtStart = false;
    public float PlayerAroundRadius = 0;
    // Start is called before the first frame update
    void Start()
    {
        damageData.damage *= Z.LS.LastInstLvl.DamageMultiplier;
        Vector2 Around = Random.insideUnitCircle * PlayerAroundRadius;
        Vector3 lookPos = Z.Player.transform.position + new Vector3(Around.x, 0, Around.y);
        lookPos.y = transform.position.y;
        transform.LookAt(lookPos);
        Debug.DrawRay(transform.position, lookPos, Color.red, 1);
        if (DestroyAtStart)
        {
            Destroy(this);
        }
    }
}
