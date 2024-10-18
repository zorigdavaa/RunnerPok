using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : MonoBehaviour
{
    [SerializeField] Transform moveObj;
    [SerializeField] float speed = 1;
    [SerializeField] float ZRot;
    [SerializeField] float ownTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        ownTime = Random.Range(0, 10);
    }

    // Update is called once per frame
    void Update()
    {
        ownTime += Time.deltaTime;
        // moveObj.localPosition = new Vector3(Mathf.PingPong(Time.time * speed, 10) - 5, moveObj.localPosition.y, moveObj.localPosition.z);
        // float angle = Mathf.PingPong(ownTime * speed, ZRot) - ZRot / 2;
        float angle = Mathf.Sin(ownTime * speed) * (ZRot / 2);
        moveObj.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
