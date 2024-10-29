using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

public class RBarriers : MonoBehaviour
{
    public List<Transform> childObjs;
    public Vector2 minMaxX;
    public List<int> Poses = new List<int>() { 0, 1, 1, 1, 1, 1 };
    List<Vector3> CurPos = new List<Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        // Old();
        Poses.Shuffle();
        Vector3 beforePos = new Vector3(-8.25f, 0, 0);
        for (int i = 0; i < Poses.Count; i++)
        {
            Vector3 pos = beforePos;
            CurPos.Add(pos);
            beforePos = pos;
            if (Poses[i] == 0)
            {
                beforePos.x += 3 + 1.5f;
            }
            else
            {
                beforePos.x += 3;
            }
        }


        for (int i = 0; i < childObjs.Count; i++)
        {
            Vector3 pos = CurPos[Random.Range(0, CurPos.Count)];
            CurPos.Remove(pos);
            childObjs[i].transform.localPosition = pos;
        }
    }

    private void Old()
    {
        minMaxX.x = -8.25f;
        minMaxX.y = 8.25f;
        childObjs = new List<Transform>();
        // childObjs = transform.GetComponentsInChildren<Transform>().ToList();
        foreach (Transform child in transform)
        {
            childObjs.Add(child);
        }
        float maxDistance = (minMaxX.x - minMaxX.y);
        float evenDistance = maxDistance / (childObjs.Count + 1);

        for (int i = 0; i < childObjs.Count + 1; i++)
        {
            CurPos.Add(new Vector3(transform.position.x - minMaxX.x + evenDistance * i, transform.position.y, transform.position.z));
        }

        for (int i = 0; i < childObjs.Count; i++)
        {
            Vector3 pos = CurPos[Random.Range(0, CurPos.Count)];
            CurPos.Remove(pos);
            childObjs[i].transform.position = pos;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
