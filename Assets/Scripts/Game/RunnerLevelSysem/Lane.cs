using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lane
{
    public float LaneXPosition;
    public List<LaneSegment> LaneSegments;
    public LaneSegment GetAtZ(float zPos)
    {
        foreach (var segment in LaneSegments)
        {
            if (zPos >= segment.Start && zPos <= segment.End)
            {
                return segment;
            }
        }
        return null;
    }

}
