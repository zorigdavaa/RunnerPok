using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [SerializeField] Transform moveObj;
    [SerializeField] float speed = 1;
    float ownTime = 0;
    float lenfth = 18;
    // Start is called before the first frame update
    void Start()
    {
        ownTime = Random.Range(0, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        ownTime += Time.deltaTime;
        moveObj.localPosition = new Vector3(Mathf.PingPong(ownTime * speed, lenfth) - lenfth/2, moveObj.localPosition.y, moveObj.localPosition.z);
        moveObj.Rotate(new Vector3(0, 0, 20));
    }
}
