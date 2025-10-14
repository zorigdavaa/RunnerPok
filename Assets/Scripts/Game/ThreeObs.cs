using System.Collections.Generic;
using UnityEngine;

public class ThreeObs : MonoBehaviour
{
    public List<Transform> Parents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var item in Parents)
        {
            int showChild = Random.Range(0, item.childCount);
            for (int i = 0; i < item.childCount; i++)
            {
                item.GetChild(i).gameObject.SetActive(i == showChild);
            }
        }
    }
}
