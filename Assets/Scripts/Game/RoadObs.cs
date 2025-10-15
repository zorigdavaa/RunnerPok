using System.Collections.Generic;
using UnityEngine;

public class RoadObs : MonoBehaviour
{
    public List<RoadObsConfig> Obstacles;
    int selectedObjIndex = -1;

    public bool Init()
    {
        selectedObjIndex = Random.Range(0, Obstacles.Count);
        Obstacles[selectedObjIndex].Obs.SetActive(true);
        foreach (var item in Obstacles)
        {
            if (item != Obstacles[selectedObjIndex])
            {
                item.Obs.SetActive(false);
            }
        }
        return Obstacles[selectedObjIndex].isBlocked;
    }

    internal void MakeUnblocked()
    {
        Obstacles[selectedObjIndex].Obs.SetActive(false);
        RoadObsConfig toActivate = null;
        toActivate = Obstacles.Find(x => x.isBlocked == false);
        if (toActivate != null)
        {
            toActivate.Obs.SetActive(true);
        }
    }
}

[System.Serializable]
public class RoadObsConfig
{
    public GameObject Obs;
    public bool isBlocked = false;
}
