using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // transform.Rotate(0, 360 * Time.deltaTime, 0);
        transform.localPosition += transform.forward * 6 * Time.deltaTime;
    }
}
