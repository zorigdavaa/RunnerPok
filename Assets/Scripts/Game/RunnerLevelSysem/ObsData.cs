using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsData : MonoBehaviour
{
    public float ownLengh;

    public void LongObsRandomness()
    {
        List<Vector3> Rotations = new List<Vector3>
        {
            new Vector3(0, 0,0),
            new Vector3(0, 180,0),
        };
        List<Vector3> Poses = new List<Vector3>
        {
            new Vector3(0, 0,0),
            new Vector3(0, 180,0),
        };
        int randomIndex = Random.Range(0, 1);
        TransformAnimation transformAnimation = GetComponent<TransformAnimation>();
        Vector3 Rotation = Rotations[randomIndex];
        transform.rotation = Quaternion.Euler(Rotation);
        transformAnimation.TargetRot = Rotation;

        transformAnimation.Pos = new Vector3(0, 0, 0);
    }
}
