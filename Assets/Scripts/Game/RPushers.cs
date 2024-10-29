using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPushers : MonoBehaviour
{
    public GameObject RightPf;
    public GameObject LeftPf;
    public int Count = 5;
    public float step = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        for (int i = 0; i < Count; i++)
        {
            pos += Vector3.forward * 7;
            GameObject right = Instantiate(RightPf, pos, RightPf.transform.rotation, transform);
            GameObject left = Instantiate(LeftPf, pos, LeftPf.transform.rotation, transform);
            right.GetComponent<SideKicker>().startOffset += step * (i + 1);
            left.GetComponent<SideKicker>().startOffset += step * (i + 1);

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
