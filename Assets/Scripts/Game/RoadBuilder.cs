using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[ExecuteInEditMode]
[RequireComponent(typeof(SplineContainer), typeof(MeshFilter), typeof(MeshRenderer))]
public class RoadBuilder : MonoBehaviour
{
    public SplineContainer splineContainer = null;
    public float width = 4f;
    public float resolution = 0.1f; // 0.1 = every 10% of spline
    public float thickness = 0.2f;
    public Mesh generatedMesh = null;

    void Start()
    {
        if (splineContainer && !generatedMesh)
        {
            GenerateRoad();
        }

    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += () =>
        {
            // Check still valid and not in Play mode
            if (this != null && splineContainer != null && !Application.isPlaying)
            {
                GenerateRoad();
            }
        };
    }
#endif


    public void GenerateRoad()
    {
        Spline spline = splineContainer.Spline;
        if (spline == null || spline.Count < 2) return;

        List<Vector2> shape2D = GetRoadShape(); // Cross-section shape
        int shapeVertsCount = shape2D.Count;

        List<Vector3> vertices = new();
        List<Vector2> uvs = new();
        List<int> triangles = new();

        int segmentCount = Mathf.CeilToInt(1f / resolution);
        float splineDistance = 0f;

        Vector3 prevPos = spline.EvaluatePosition(0f);

        // --- Generate vertices ---
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = i / (float)segmentCount;
            Vector3 pos = spline.EvaluatePosition(t);
            Vector3 forward = spline.EvaluateTangent(t);
            Vector3 up = Vector3.up;
            Vector3 right = Vector3.Cross(up, forward).normalized;

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

        // --- Build triangles ---
        for (int seg = 0; seg < segmentCount; seg++)
        {
            int baseIndex = seg * shapeVertsCount;
            int nextBaseIndex = (seg + 1) * shapeVertsCount;

            for (int i = 0; i < shapeVertsCount - 1; i++)
            {
                triangles.Add(baseIndex + i + 1);
                triangles.Add(nextBaseIndex + i);
                triangles.Add(baseIndex + i);

                triangles.Add(baseIndex + i + 1);
                triangles.Add(nextBaseIndex + i + 1);
                triangles.Add(nextBaseIndex + i);

            }

            // close side face (last to first in ring)
            triangles.Add(baseIndex);
            triangles.Add(nextBaseIndex + shapeVertsCount - 1);
            triangles.Add(baseIndex + shapeVertsCount - 1);

            triangles.Add(baseIndex);
            triangles.Add(nextBaseIndex);
            triangles.Add(nextBaseIndex + shapeVertsCount - 1);

        }

        // --- Front cap ---
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
            triangles.Add(next);
            triangles.Add(i);
        }

        // --- Back cap ---
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
            triangles.Add(lastRingStart + i);
            triangles.Add(lastRingStart + next);
        }

        // --- Create mesh ---
        Mesh mesh = new Mesh();
        mesh.name = "RoadMesh";
        mesh.SetVertices(vertices);
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();

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
                UnityEditor.AssetDatabase.SaveAssets();
            }
            else
            {
                UnityEditor.AssetDatabase.CreateAsset(mesh, assetPath);
                UnityEditor.AssetDatabase.SaveAssets();
            }

        }

        GetComponent<MeshFilter>().sharedMesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        generatedMesh = mesh;

    }
    List<Vector2> GetRoadShape()
    {
        float w = width * 0.5f;
        float t = -thickness;
        return new List<Vector2>()
    {
        new Vector2(-w, t), // bottom-left
        new Vector2(w, t),  // bottom-right
        new Vector2(w, 0),  // top-right
        new Vector2(-w, 0), // top-left
        // new Vector2(-w, t) // Optional: duplicate start for closed loop
    };
    }

}
