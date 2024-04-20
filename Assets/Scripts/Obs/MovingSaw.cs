using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [SerializeField] Transform moveObj;
    [SerializeField] float speed = 1;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        moveObj.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, 10) - 5, moveObj.localPosition.y, moveObj.localPosition.z);
        moveObj.Rotate(new Vector3(0, 0, 20));
    }
}
