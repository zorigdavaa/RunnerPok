using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityUtilities;

public class RBarriers : MonoBehaviour
{
    public List<Transform> childObjs;
    public List<int> Poses = new List<int>() { 0, 1, 1, 1, 1, 1 };
    List<Vector3> CurPos = new List<Vector3>();
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

        float startX = -8.25f;
        float maxX = 8.25f;
        float x = startX;

        for (int i = 0; i < Poses.Count; i++)
        {
            // Add current position
            CurPos.Add(new Vector3(x, 0, 0));

            // Decide spacing
            float spacing;

            if (Poses[i] == 0)
            {
                // Open: use wider spacing
                spacing = 3f + 1.5f;
            }
            else
            {
                // Could be open or overlapping
                spacing = Random.value < 0.8f ? 3f : Random.Range(1.5f, 3.5f);
            }

            // Update X for next position
            x += spacing;

            // Optional: keep within bounds
            if (x > maxX)
            {
                x = maxX - Random.Range(1.5f, 3.5f); // Pull back to fit
            }
        }
        for (int i = 0; i < childObjs.Count; i++)
        {
            Vector3 pos = CurPos[Random.Range(0, CurPos.Count)];
            CurPos.Remove(pos);
            childObjs[i].transform.localPosition = pos;
        }
    }



    // Update is called once per frame
    void Update()
    {

    }
}
