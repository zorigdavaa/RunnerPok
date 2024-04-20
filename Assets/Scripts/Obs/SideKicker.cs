using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideKicker : MonoBehaviour
{
    [SerializeField] Transform Kicker;
    public float forwardSpeed = 5f;         // Speed when moving forward
    public float backwardSpeed = 1f;        // Speed when moving backward
    public float maxForwardDistance = 5f;   // Maximum distance to move forward

    private bool movingForward = true;      // Flag to track movement direction

    void Start()
    {

    }

    void Update()
    {
        // Move the obstacle
        if (movingForward)
        {
            // Move forward
            Kicker.localPosition -= Vector3.right * forwardSpeed * Time.deltaTime;

            // Check if reached max distance
            if (Kicker.localPosition.x < 0)
            {
                movingForward = false; // Change direction
            }
        }
        else
        {
            // Move backward
            Kicker.localPosition += Vector3.right * backwardSpeed * Time.deltaTime;

            // Check if reached initial position
            if (Kicker.localPosition.x > maxForwardDistance)
            {
                movingForward = true; // Change direction
            }
        }
    }
}
