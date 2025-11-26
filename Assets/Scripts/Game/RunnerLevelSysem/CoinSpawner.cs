using System.Collections.Generic;
using GameAnalyticsSDK.Setup;
using UnityEngine;

public class CoinSpawner
{
    public GameObject InsPF;
    public void InsObjRaycast(int count, Transform parent, List<Lane> lanes, Vector3 initPos)
    {
        Lane lane = lanes[Random.Range(0, lanes.Count)];
        for (int i = 0; i < count; i++)
        {
            initPos.x = lane.LaneXPosition;
            if (Physics.Raycast(initPos + Vector3.up * 50, Vector3.down, out RaycastHit hitInfo, 100f, LayerMask.GetMask("Road")))
            {
                initPos.y = hitInfo.point.y + 1f;
            }
            Object.Instantiate(InsPF, initPos, Quaternion.identity, parent);
            initPos += Vector3.forward * 5;
            if (i % 5 == 0)
            {
                lane = lanes[Random.Range(0, lanes.Count)];
            }
        }
    }
    public CoinSpawner(GameObject pf)
    {
        InsPF = pf;
    }
}
