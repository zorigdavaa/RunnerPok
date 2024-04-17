using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour, ICollisionAction
{
    public void CollisionAction(Character character)
    {
        character.GetComponent<Rigidbody>().velocity = Vector3.up * 10 + Vector3.forward * 10;
        // character.GetComponent<Rigidbody>().AddForce(Vector3.up * 400);
    }
}
