using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LocalTrail : MonoBehaviour
{
    public float trailLength = 5f; // Length of the trail in seconds
    public float trailWidth = 0.2f; // Width of the trail

    private Mesh trailMesh;
    private List<Vector3> points = new List<Vector3>();
    private List<float> times = new List<float>();

    void Start()
    {
        trailMesh = new Mesh();
        GetComponent<MeshFilter>().mesh = trailMesh;
    }

    void Update()
    {
        // Add the current local position to the trail
        points.Add(transform.localPosition);
        times.Add(Time.time);

        // Remove old points based on trail length
        while (times.Count > 0 && Time.time - times[0] > trailLength)
        {
            points.RemoveAt(0);
            times.RemoveAt(0);
        }

        // Update the mesh
        UpdateTrailMesh();
    }

    private void UpdateTrailMesh()
    {
        if (points.Count < 2)
        {
            trailMesh.Clear();
            return;
        }

        // Create vertex and triangle arrays
        Vector3[] vertices = new Vector3[points.Count * 2];
        int[] triangles = new int[(points.Count - 1) * 6];
        Vector2[] uvs = new Vector2[vertices.Length];

        for (int i = 0; i < points.Count; i++)
        {
            // Compute the direction of the trail
            Vector3 direction = i == 0 ? points[1] - points[0] : points[i] - points[i - 1];
            Vector3 perpendicular = Vector3.Cross(direction.normalized, Vector3.forward) * trailWidth * 0.5f;

            // Create two vertices for each point
            vertices[i * 2] = points[i] - perpendicular;
            vertices[i * 2 + 1] = points[i] + perpendicular;

            // Set UV coordinates
            float uvX = (float)i / (points.Count - 1);
            uvs[i * 2] = new Vector2(uvX, 0);
            uvs[i * 2 + 1] = new Vector2(uvX, 1);

            // Create triangles
            if (i < points.Count - 1)
            {
                int t = i * 6;
                int v = i * 2;

                triangles[t] = v;
                triangles[t + 1] = v + 2;
                triangles[t + 2] = v + 1;

                triangles[t + 3] = v + 1;
                triangles[t + 4] = v + 2;
                triangles[t + 5] = v + 3;
            }
        }

        // Assign data to the mesh
        trailMesh.Clear();
        trailMesh.vertices = vertices;
        trailMesh.triangles = triangles;
        trailMesh.uv = uvs;
        trailMesh.RecalculateNormals();
    }
}
