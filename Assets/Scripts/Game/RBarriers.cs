using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RBarriers : MonoBehaviour
{
    List<GameObject> childObjs;
    public Vector2 minMaxX;
    List<Vector3> Poses;
    // Start is called before the first frame update
    void Start()
    {
        minMaxX.x = -5;
        minMaxX.y = 5;
        childObjs = transform.GetComponentsInChildren<GameObject>().ToList();
        float maxDistance = (minMaxX.x - minMaxX.y);
        float evenDistance = maxDistance / childObjs.Count;
        for (int i = 0; i < childObjs.Count; i++)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
