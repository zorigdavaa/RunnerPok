using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSkinMesh : MonoBehaviour
{
    public SkinnedMeshRenderer[] Pf;
    public SkinnedMeshRenderer Body;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in Pf)
        {
            var insObj = Instantiate(item, transform.position, Quaternion.identity);
            insObj.transform.SetParent(transform);
            insObj.rootBone = Body.rootBone;
            insObj.bones = Body.bones;
        }


    }

}
