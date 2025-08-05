using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;


[ExecuteAlways]
[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadBuilder : MonoBehaviour
{
    public bool UseDefaultPoint = true;
    public List<Vector2> MeshPoints = new List<Vector2>();
    public SplineContainer splineContainer = null;
    public float width = 4f;
    [Range(0.001f, 1f)]
    public float resolution = 0.1f; // 0.1 = every 10% of spline
    public float thickness = 0.2f;
    public Mesh generatedMesh = null;
    bool shouldRegenerate = false;
    /// <summary>The main Spline to extrude.</summary>

    void Start()
    {
        if (splineContainer && !generatedMesh)
        {
            GenerateRoad();
        }
    }
    void Update()
    {
        if (shouldRegenerate)
        {
            shouldRegenerate = false;
            GenerateRoad();
        }
    }
    void OnEnable()
    {
        GenerateRoad();
        Spline.Changed += OnSplineChanged;
    }

    private void OnSplineChanged(Spline spline, int arg2, SplineModification modification)
    {
        if (splineContainer.Spline == spline)
        {
            // If the spline changed, we need to regenerate the road mesh
            shouldRegenerate = true;

        }
    }

    void OnDisable()
    {
        Spline.Changed -= OnSplineChanged;
        CleanUp();
    }

    private void CleanUp()
    {
        if (generatedMesh)
        {
            DestroyImmediate(generatedMesh);
            generatedMesh = null;
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (gameObject.activeSelf)
        {
            shouldRegenerate = true;
        }
    }
#endif


    public void GenerateRoad()
    {
        Spline spline = splineContainer.Spline;
        if (spline == null || spline.Count < 2) return;

        List<Vector2> shape2D = GetRoadShape(); // Cross-section shape (like a horizontal line)
        int shapeVertsCount = shape2D.Count;

        List<Vector3> vertices = new();
        List<Vector2> uvs = new();
        List<int> triangles = new();

        int segmentCount = Mathf.CeilToInt(1f / resolution);
        float splineDistance = 0f;

        Vector3 prevPos = spline.EvaluatePosition(0f);

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 pos = spline.EvaluatePosition(t);
            Vector3 tangent = spline.EvaluateTangent(t);

            // Calculate orientation frame (Frenet frame)
            Vector3 normal = Vector3.up;
            if (Mathf.Abs(Vector3.Dot(tangent, normal)) > 0.99f)
                normal = Vector3.forward; // Avoid gimbal lock

            Vector3 right = Vector3.Cross(normal, tangent).normalized;
            Vector3 up = Vector3.Cross(tangent, right).normalized;

            if (i > 0)
                splineDistance += Vector3.Distance(pos, prevPos);
            prevPos = pos;

            for (int j = 0; j < shapeVertsCount; j++)
            {
                Vector2 p = shape2D[j];
                Vector3 world = pos + right * p.x + up * p.y;
                vertices.Add(world);

                float u = (p.x + (width * 0.5f)) / width;
                float v = splineDistance;
                uvs.Add(new Vector2(u, v));
            }
        }

        // Generate triangles
        for (int seg = 0; seg < segmentCount; seg++)
        {
            int baseIndex = seg * shapeVertsCount;
            int nextBaseIndex = (seg + 1) * shapeVertsCount;

            for (int i = 0; i < shapeVertsCount - 1; i++)
            {
                triangles.Add(baseIndex + i);
                triangles.Add(nextBaseIndex + i);
                triangles.Add(baseIndex + i + 1);

                triangles.Add(baseIndex + i + 1);
                triangles.Add(nextBaseIndex + i);
                triangles.Add(nextBaseIndex + i + 1);
            }

            // Close the ring
            triangles.Add(baseIndex + shapeVertsCount - 1);
            triangles.Add(nextBaseIndex + shapeVertsCount - 1);
            triangles.Add(baseIndex);

            triangles.Add(baseIndex);
            triangles.Add(nextBaseIndex + shapeVertsCount - 1);
            triangles.Add(nextBaseIndex);
        }

        // Add front cap
        Vector3 frontCenter = Vector3.zero;
        for (int i = 0; i < shapeVertsCount; i++)
            frontCenter += vertices[i];
        frontCenter /= shapeVertsCount;
        int frontCenterIndex = vertices.Count;
        vertices.Add(frontCenter);
        uvs.Add(new Vector2(0.5f, 0f));

        for (int i = 0; i < shapeVertsCount; i++)
        {
            int next = (i + 1) % shapeVertsCount;
            triangles.Add(frontCenterIndex);
            triangles.Add(i);
            triangles.Add(next);
        }

        // Add back cap
        int lastRingStart = segmentCount * shapeVertsCount;
        Vector3 backCenter = Vector3.zero;
        for (int i = 0; i < shapeVertsCount; i++)
            backCenter += vertices[lastRingStart + i];
        backCenter /= shapeVertsCount;
        int backCenterIndex = vertices.Count;
        vertices.Add(backCenter);
        uvs.Add(new Vector2(0.5f, splineDistance));

        for (int i = 0; i < shapeVertsCount; i++)
        {
            int next = (i + 1) % shapeVertsCount;
            triangles.Add(backCenterIndex);
            triangles.Add(lastRingStart + next);
            triangles.Add(lastRingStart + i);
        }

        // Create and assign mesh
        Mesh mesh = new Mesh();
        mesh.name = "RoadMesh";
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            string folderPath = "Assets/GeneratedRoads";
            string meshName = gameObject.name + "_RoadMesh";
            string assetPath = $"{folderPath}/{meshName}.asset";

            System.IO.Directory.CreateDirectory(folderPath);
            Mesh existing = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(assetPath);
            if (existing != null)
            {
                UnityEditor.EditorUtility.CopySerialized(mesh, existing);
            }
            else
            {
                UnityEditor.AssetDatabase.CreateAsset(mesh, assetPath);
            }
            UnityEditor.AssetDatabase.SaveAssets();
        }
#endif

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        generatedMesh = mesh;
    }
    List<Vector2> GetRoadShape()
    {
        if (UseDefaultPoint)
        {
            float w = width * 0.5f;
            float t = -thickness;
            return new List<Vector2>()
            {
                new Vector2(-w, t), // bottom-left
                new Vector2(-w, 0), // top-left
                new Vector2(w, 0),  // top-right
                new Vector2(w, t),  // bottom-right
                // new Vector2(-w, t) // Optional: duplicate start for closed loop
            };
        }
        else
        {
            return MeshPoints;
        }

    }

}
