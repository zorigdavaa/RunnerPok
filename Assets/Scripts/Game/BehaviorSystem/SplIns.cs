using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Splines;
using UnityUtilities;
using Random = UnityEngine.Random;
using RangeInt = UnityUtilities.RangeInt;


[ExecuteInEditMode]
public class SplIns : MonoBehaviour
{

    [Serializable]
    public struct ChancePF
    {
        public GameObject Prefab;
        public float Probability;
        public float additionalDistance;
        public Vector3Offset posOffset;
        // public Vector3Offset rotOffset;
    }

    [Serializable]
    public struct Vector3Offset
    {
        public Vector3 min;
        public Vector3 max;
        internal Vector3 GetNextOffset()
        {
            return new Vector3(
                Random.Range(min.x, max.x),
                Random.Range(min.y, max.y),
                Random.Range(min.z, max.z));
        }
    }

    [SerializeField] SplineContainer m_Container;
    public SplineContainer Container
    {
        get => m_Container;
        set => m_Container = value;
    }
    [SerializeField]
    List<ChancePF> m_ItemsToInstantiate = new List<ChancePF>();
    public ChancePF[] itemsToInstantiate
    {
        get => m_ItemsToInstantiate.ToArray();
        set
        {
            m_ItemsToInstantiate.Clear();
            m_ItemsToInstantiate.AddRange(value);
        }
    }
    public bool m_IsDirty = false;
    public int m_Seed = 0;
    [SerializeField] int count = -1;
    public int RandomCount
    {
        get
        {

            count = CountRange.RandomInclusive;

            return count;
        }
        set { count = value; }
    }

    public RangeInt CountRange;
    public Vector3Offset posOffset;
    public float maxDistance = 0;
    public float m_MaxProbability = 1;

    void OnSplineChanged(Spline spline, int knotIndex, SplineModification modificationType)
    {
        if (m_Container != null && m_Container.Spline == spline)
            m_IsDirty = true;
    }

    void OnEnable()
    {
        bool flowControl = Init();
        if (!flowControl)
        {
            Debug.LogError("Error Flow");
            return;
        }
        if (m_Seed == 0)
            m_Seed = Random.Range(int.MinValue, int.MaxValue);
        Spline.Changed += OnSplineChanged;

    }

    private bool Init()
    {
        if (m_Container == null)
        {
            m_Container = GetComponent<SplineContainer>();
            if (m_Container == null)
            {
                m_Container = transform.parent.GetComponentInChildren<SplineContainer>();
            }
            if (m_Container == null)
            {
                return false;
            }
        }
        maxDistance = 0f;
        for (int splineIndex = 0; splineIndex < m_Container.Splines.Count; splineIndex++)
        {
            var length = m_Container.CalculateLength(splineIndex);
            maxDistance += length;
        }
        m_MaxProbability = 0;
        for (int i = 0; i < itemsToInstantiate.Length; i++)
        {
            m_MaxProbability += itemsToInstantiate[i].Probability;
        }

        return true;
    }

    void OnDisable()
    {
        DeleteInsItems();
        Spline.Changed -= OnSplineChanged;
    }
    void OnValidate()
    {
        Init();
        // InstantiateAsChild();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateAsChild();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsDirty)
        {
            m_IsDirty = false;
            InstantiateAsChild();
        }
    }
    public float averageDistance;
    public List<GameObject> InsItems = new List<GameObject>();
    [ContextMenu("Ins")]
    private void InstantiateAsChild()
    {
        DeleteInsItems();
        float currentDistance = 5f;
        float clampedMax = maxDistance - 5;
        averageDistance = clampedMax / RandomCount;
        for (int i = 0; i < count; i++)
        {
            if (currentDistance > clampedMax)
            {
                break;
            }
            int index = GetPrefabIndex();
            ChancePF chancePF = itemsToInstantiate[index];
            currentDistance += chancePF.additionalDistance;
            Vector3 insPOS = Container.Spline.GetPointAtLinearDistance(0, currentDistance, out float resultPointT);
            Container.Evaluate(resultPointT, out float3 pos, out float3 tangent, out float3 up);
            Quaternion rotation = Quaternion.LookRotation(tangent, up);
            currentDistance += averageDistance;
            GameObject pf = chancePF.Prefab;
            GameObject Ins_obj = Instantiate(pf, Vector3.zero, Quaternion.identity, transform);

            // var remappedForward = math.normalizesafe(GetAxis(1));
            // var remappedUp = math.normalizesafe(GetAxis(2));
            // var axisRemapRotation = Quaternion.Inverse(quaternion.LookRotationSafe(remappedForward, remappedUp));

            // Quaternion Rot = quaternion.LookRotationSafe(forward, up) * axisRemapRotation;
            Ins_obj.transform.localPosition = insPOS;
            Ins_obj.transform.localRotation = rotation;
            InsItems.Add(Ins_obj);
        }
    }

    private void DeleteInsItems()
    {
        foreach (var item in InsItems)
        {
#if UNITY_EDITOR
            DestroyImmediate(item);
#else
            Destroy(item);
#endif
        }
        InsItems.Clear();
    }

    int GetPrefabIndex()
    {
        var prefabChoice = Random.Range(0, m_MaxProbability);
        var currentProbability = 0f;
        for (int i = 0; i < m_ItemsToInstantiate.Count; i++)
        {
            if (m_ItemsToInstantiate[i].Prefab == null)
                continue;

            var itemProbability = m_ItemsToInstantiate[i].Probability;
            if (prefabChoice < currentProbability + itemProbability)
                return i;

            currentProbability += itemProbability;
        }

        return 0;
    }
    // private readonly float3[] m_AlignAxisToVector = new float3[6]
    // {
    //         math.right(),
    //         math.up(),
    //         math.forward(),
    //         math.left(),
    //         math.down(),
    //         math.back()
    // };

    // protected float3 GetAxis(int i)
    // {
    //     return m_AlignAxisToVector[i];
    // }
}
