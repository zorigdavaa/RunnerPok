using UnityEngine;

public class UpHill : ObsData
{

    // GameObject TriAngleUpHill;
    GameObject Body;
    Transform End;
    float segmentLength = 20f;
    void Awake()
    {
        // TriAngleUpHill = transform.Find("TriAngle").gameObject;
        Body = transform.Find("Body").gameObject;
        End = transform.Find("End");
        segmentLength = End.position.z - transform.position.z;
        ownLengh = segmentLength;
    }

    public void ExtendUphill(int times)
    {
        Debug.Log("Extend Uphill Called");
        for (int i = 0; i < times; i++)
        {
            GameObject InsBody = Instantiate(Body, End.position, Quaternion.identity, transform);
            Vector3 scale = InsBody.transform.localScale;
            scale.z += 0.0001f;
            InsBody.transform.localScale = scale;
            End.position += new Vector3(0, 0, segmentLength);
            ownLengh += segmentLength;
        }
    }
}
