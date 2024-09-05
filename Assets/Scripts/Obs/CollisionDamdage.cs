using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class CollisionDamdage : MonoBehaviour, ICollisionAction
{
    public float Damage = 5;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            player.TakeDamage(-Damage);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Damage *= Z.LS.LastInstLvl.DamageMultiplier;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
