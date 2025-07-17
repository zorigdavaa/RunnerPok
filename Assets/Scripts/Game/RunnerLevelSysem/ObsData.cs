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
        Vector3 Rotation = Rotations[Random.Range(0, 1)];
        transform.rotation = Quaternion.Euler(Rotation);

        TransformAnimation transformAnimation = GetComponent<TransformAnimation>();
        transformAnimation.Pos = new Vector3(0, 0, 0);
    }
}
