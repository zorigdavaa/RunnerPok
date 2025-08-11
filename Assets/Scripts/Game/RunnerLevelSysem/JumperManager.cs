using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JumperManager : MonoBehaviour
{
    List<Jumper> Jumpers = new List<Jumper>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Jumpers = GetComponentsInChildren<Jumper>().OrderBy(x => x.transform.position.z).ToList();
        Transform end = transform.Find("NewEnd");
        if (end == null)
        {
            end = transform.Find("End");
        }
        for (int i = 0; i < Jumpers.Count; i++)
        {
            Jumper current = Jumpers[i];
            Transform taget;
            if (i < Jumpers.Count - 1)
            {
                taget = Jumpers[i + 1].transform;

            }
            else
            {
                taget = end.transform;
            }
            current.NextJumper = taget;
        }
    }
}
