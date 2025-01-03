using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class PlayerNearAnimation : MonoBehaviour
{
    Player player;
    public bool Animated = false;
    public Vector3 TargerPos;
    public Transform MoveObj;
    public bool UseLocalPos = false;
    public float PlayerNearDistance = 20;
    public float Duration = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        player = Z.Player;
        if (MoveObj == null)
        {
            MoveObj = transform;
            // TargerPos += transform.position;
        }
        if (UseLocalPos)
        {
            TargerPos += transform.localPosition;
        }
        else
        {
            TargerPos += transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > transform.position.z - PlayerNearDistance && !Animated)
        {
            Animated = true;
            DOAnimation();
        }
    }

    private void DOAnimation()
    {
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            float t = 0;
            float time = 0;
            Vector3 initialPosition;
            if (UseLocalPos)
            {
                initialPosition = MoveObj.localPosition;
            }
            else
            {
                initialPosition = MoveObj.position;
            }

            while (time < Duration)
            {
                time += Time.deltaTime;
                t = time / Duration;
                if (UseLocalPos)
                {
                    MoveObj.localPosition = Vector3.Lerp(initialPosition, TargerPos, t);
                }
                else
                {
                    MoveObj.position = Vector3.Lerp(initialPosition, TargerPos, t);
                }
                yield return null;
            }
        }
    }
}
