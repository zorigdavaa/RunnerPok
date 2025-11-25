using UnityEngine;

public class UpHill : MonoBehaviour
{
    // GameObject TriAngleUpHill;
    GameObject Body;
    Transform End;
    void Awake()
    {
        // TriAngleUpHill = transform.Find("TriAngle").gameObject;
        Body = transform.Find("Body").gameObject;
        End = transform.Find("End");
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ExtendUphill(int times)
    {
        Debug.Log("Extend Uphill Called");
        for (int i = 0; i < times; i++)
        {
            Instantiate(Body, End.position, Quaternion.identity, transform);
            End.position += new Vector3(0, 0, 20);
        }
    }
}
