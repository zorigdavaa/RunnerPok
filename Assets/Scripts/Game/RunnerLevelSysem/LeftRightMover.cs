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
        float t = Mathf.InverseLerp(-1, 1, Mathf.Sin(ownTime * speed));
        Model.localPosition = Vector3.Lerp(min, max, t);
    }
}
