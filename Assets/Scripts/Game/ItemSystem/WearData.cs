using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class WearData
{
    public Vector3 Position;
    public Vector3 Rotation;
    public Quaternion GetRotation => Quaternion.Euler(Rotation);
    // public WhereSlot where;
    public GameObject PF;
}
