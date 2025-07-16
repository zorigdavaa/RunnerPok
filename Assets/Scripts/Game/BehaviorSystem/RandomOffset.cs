using UnityEngine;
using UnityUtilities;

public class RandomOffset : MonoBehaviour
{
    public bool UseRangePos = false;
    public RangeFloat rangeX;
    public RangeFloat rangeY;
    public RangeFloat rangeZ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (UseRangePos)
        {
            Vector3 pos = transform.position;
            pos.x += rangeX.RandomInclusive;
            pos.y += rangeY.RandomInclusive;
            pos.z += rangeZ.RandomInclusive;
            transform.position = pos;
        }
        Destroy(this);

    }
    
}
