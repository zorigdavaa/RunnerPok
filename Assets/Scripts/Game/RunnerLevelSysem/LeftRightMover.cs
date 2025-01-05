using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftRightMover : MonoBehaviour
{
    [SerializeField] Transform Model;
    float ownTime;
    public float speed = 5;
    public bool Oppozit;
    [Range(0, Mathf.PI / 2)]
    public float startOffset = 0;
    public MovementType MovementType;
    void Start()
    {
        if (!Model)
        {
            Model = transform;
        }
        if (Oppozit)
        {
            ownTime = Mathf.PI / 2;
        }
        ownTime += startOffset;
    }
    public Vector3 min = new Vector3(0, 0, 0);
    public Vector3 max = new Vector3(7, 0, 0);
    void Update()
    {
        ownTime += Time.deltaTime;
        float t;
        if (MovementType == MovementType.SinusWave)
        {
            t = Mathf.InverseLerp(-1, 1, Mathf.Sin(ownTime * speed));
        }
        else
        {
            t = Mathf.PingPong(ownTime * speed, 1f); // Creates a linear oscillation between 0 and 1
        }
        Model.localPosition = Vector3.Lerp(min, max, t);
    }
}
public enum MovementType
{
    SinusWave,
    Linear
}
