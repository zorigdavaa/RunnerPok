using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        Projectile proj = other.transform.GetComponent<Projectile>();
        if (proj != null)
        {
            Destroy(proj.gameObject);
        }
    }
}
