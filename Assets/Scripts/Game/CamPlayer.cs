using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayer : MonoBehaviour
{
    [SerializeField] AnimationController animationController;
    // Start is called before the first frame update
    void Start()
    {
        animationController.SetSpeed(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {

    }
}
