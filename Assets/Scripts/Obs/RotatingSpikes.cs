using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class RotatingSpikes : MonoBehaviour
{
    [SerializeField] Transform RotObsj;
    [SerializeField] Vector3 RotationAngle;
    [SerializeField] Vector3 LocalRotation;
    [SerializeField] float speed = 5;

    public float radius = 1f;
    public int segments = 30;
    public float width = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        radius = Vector3.Distance(transform.position, RotObsj.position);
        if (radius > 0.1)
        {

            CreateCircle();
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotObsj.RotateAround(transform.position, RotationAngle, speed);
        RotObsj.Rotate(LocalRotation);
    }
    void CreateCircle()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 6];

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.PI * 2f / segments * i;
            float x = Mathf.Cos(angle);
            float y = Mathf.Sin(angle);
            Debug.Log("Angle for segment " + i + ": " + angle + " degrees");
            vertices[i * 2] = new Vector3(x * (radius - width * 0.5f), 0.01f, y * (radius - width * 0.5f));
            vertices[i * 2 + 1] = new Vector3(x * (radius + width * 0.5f), 0.01f, y * (radius + width * 0.5f));

            int triIndex = i * 6;  // 0,6,12
            triangles[triIndex] = i * 2; // 0,2,4
            triangles[triIndex + 1] = (i * 2 + 2) % (segments * 2);
            triangles[triIndex + 2] = i * 2 + 1;
            triangles[triIndex + 3] = i * 2 + 1;
            triangles[triIndex + 4] = (i * 2 + 2) % (segments * 2);
            triangles[triIndex + 5] = (i * 2 + 3) % (segments * 2);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
