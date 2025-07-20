using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObsData : MonoBehaviour
{
    public float ownLengh;
    public ObsDataType obsData;

    public void LongObsRandomness()
    {
        List<Vector3> Rotations = new List<Vector3>
        {
            new Vector3(0, 0,0),
            new Vector3(0, 180,0),
        };
        // List<Vector3> Poses = new List<Vector3>
        // {
        //     new Vector3(0, 0,0),
        //     new Vector3(0, 180,0),
        // };
        int randomIndex = Random.Range(0, 1);
        TransformAnimation transformAnimation = GetComponent<TransformAnimation>();
        Vector3 Rotation = Rotations[randomIndex];
        transform.localRotation = transform.localRotation * Quaternion.Euler(Rotation);
        transformAnimation.TargetRot = transform.localRotation.eulerAngles;

        transformAnimation.Pos = new Vector3(0, 0, 0);
    }
    public void ObsDataMethod()
    {
        switch (obsData)
        {
            case ObsDataType.Long:
                LongObsRandomness(); break;

            default:
                break;
        }
    }
}
public enum ObsDataType
{
    None, Long
}
