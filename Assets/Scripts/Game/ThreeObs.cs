using System.Collections.Generic;
using UnityEngine;

public class ThreeObs : MonoBehaviour
{
    public List<RoadObs> RoadObss;
    public List<bool> isBlockedList = new List<bool>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init();

    }

    private void Init()
    {
        isBlockedList = new List<bool>();
        foreach (var item in RoadObss)
        {
            bool isBlocked = item.Init();
            isBlockedList.Add(isBlocked);
        }
        if (!isBlockedList.Contains(false))
        {
            int randIndex = Random.Range(0, isBlockedList.Count);
            isBlockedList[randIndex] = false;
            RoadObss[randIndex].MakeUnblocked();
        }
    }
    [ContextMenu("Test")]
    public void Test()
    {
        Init();
    }
}


