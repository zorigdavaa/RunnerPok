using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

public class RBarriers : MonoBehaviour
{
    public List<Transform> childObjs;
    public List<int> Poses = new List<int>() { 0, 1, 1, 1 };
    List<Vector3> CurPos = new List<Vector3>();
    public float Edge = 7.5f;
    public float spacing = 5;
    // Start is called before the first frame update
    void Start()
    {
        // Old();
        GeneratePositions();
    }
    [ContextMenu("Placement")]
    private void GeneratePositions()
    {
        CurPos = new List<Vector3>();
        Poses.Shuffle();
        float x = -Edge;

        for (int i = 0; i < Poses.Count; i++)
        {
            // Add current position
            CurPos.Add(new Vector3(x, 0, 0));
            // Update X for next position
            x += spacing;

        }
        List<Vector3> usedPositions = new();

        for (int i = 0; i < Poses.Count; i++)
        {
            Vector3 pos = CurPos[Random.Range(0, CurPos.Count)];

            if (usedPositions.Contains(pos))
            {
                childObjs[i].gameObject.SetActive(false);
                continue;
            }

            usedPositions.Add(pos);

            if (Poses[i] == 0)
            {
                childObjs[i].gameObject.SetActive(false);
            }
            else
            {
                childObjs[i].transform.localPosition = pos;
                childObjs[i].gameObject.SetActive(true);
            }
        }

    }
}
