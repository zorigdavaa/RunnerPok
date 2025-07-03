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
            // ReleaseObj();
        }
    }

    public void ReleaseObj()
    {
        Roller.isKinematic = false;
        Roller.linearVelocity = Vector3.back * 50;
        Roller.angularVelocity = new Vector3(10, 0, 0);
        if (GetComponent<LeftRightMover>() != null)
        {
            GetComponent<LeftRightMover>().IsMoving = false;
        }
    }
}
