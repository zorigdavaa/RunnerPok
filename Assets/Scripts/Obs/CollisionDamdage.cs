using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class CollisionDamdage : MonoBehaviour, ICollisionAction
{
    public float Damage = 5;
    public GameObject feathersVFX;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            player.TakeDamage(-Damage);
            if (feathersVFX)
            {

                GameObject particle = Instantiate(feathersVFX, transform.position, Quaternion.identity);
                Destroy(particle, 2);
            }
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return null;
        Damage *= Z.LS.LastInstLvl.DamageMultiplier;
        feathersVFX = Resources.Load<GameObject>("Game/FeatherExplosion.prefab");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
