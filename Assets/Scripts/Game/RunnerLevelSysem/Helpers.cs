using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    public static float FixValue(float v)
    {
        float min = 1.0f;
        // float max = 10.0f;
        if (v < -min || v > min) return v;   // clamp negative
        return v < 0 ? -min : min;     // collapse inner zone
    }

    public static void InsObjRaycast(int count, Transform parent, List<Lane> lanes, Vector3 initPos, GameObject InsPf)
    {
        Lane lane = lanes[Random.Range(0, lanes.Count)];
        for (int i = 0; i < count; i++)
        {
            initPos.x = lane.LaneXPosition;
            if (Physics.Raycast(initPos + Vector3.up * 50, Vector3.down, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Road")))
            {
                initPos.y = hitInfo.point.y + 1f;
            }
            Object.Instantiate(InsPf, initPos, Quaternion.identity, parent);
            initPos += Vector3.forward * 5;
            if (i % 5 == 0)
            {
                lane = lanes[Random.Range(0, lanes.Count)];
            }
        }
    }
}
