using System.Collections.Generic;
using UnityEngine;


//Complex Section
public class CSection : LevelSection
{
    public List<GameObject> Obstacles;

    public override void StartSection(Level level)
    {
        Reset();
        base.StartSection(level);
    }
    int NumberOfTiles = 20;

    // Update is called once per frame
    void Update()
    {

    }
}
