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
        StartCoroutine(LocalCor());
    }
    IEnumerator LocalCor()
    {
        Roller.isKinematic = false;
        // Roller.linearVelocity = Vector3.back * 60;
        // Roller.angularVelocity = new Vector3(10, 0, 0);
        if (GetComponent<LeftRightMover>() != null)
        {
            GetComponent<LeftRightMover>().IsMoving = false;
        }
        float t = 0;
        float time = 0;
        float duration = 3f;
        Vector3 initialPosition = Roller.transform.localPosition;
        Vector3 targetPosition = initialPosition + Vector3.back * 100;
        while (time < duration)
        {
            time += Time.deltaTime;
            t = time / duration;
            Roller.transform.localPosition = Vector3.Lerp(initialPosition, targetPosition, t);
            yield return null;
        }
    }
}
