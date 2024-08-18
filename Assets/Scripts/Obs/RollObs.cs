using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class RollObs : MonoBehaviour, ICollisionAction
{
    [SerializeField] Rigidbody Roller;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            Roller.isKinematic = false;
            Roller.velocity = Vector3.back * 30;
            Roller.angularVelocity = new Vector3(10, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Roller.transform.position += Vector3.right * Random.Range(-4, 4);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
