using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamdage : MonoBehaviour, ICollisionAction
{
    public int Damage = 5;
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

    }

    // Update is called once per frame
    void Update()
    {

    }
}
