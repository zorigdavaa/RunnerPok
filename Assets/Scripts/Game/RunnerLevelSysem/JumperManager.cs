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
        Transform end = transform.Find("End");
        for (int i = 0; i < Jumpers.Count; i++)
        {
            Jumper current = Jumpers[i];
            Vector3 taget;
            if (i < Jumpers.Count - 1)
            {
                taget = Jumpers[i + 1].transform.position;

            }
            else
            {
                taget = end.transform.position;
            }
            current.NextJumpTarget = taget;
        }
    }
}
