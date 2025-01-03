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
        if (player.transform.position.z > transform.position.z - 20 && !Animated)
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
            float duration = 0.5f;
            Vector3 initialPosition;
            if (UseLocalPos)
            {
                initialPosition = MoveObj.localPosition;
            }
            else
            {
                initialPosition = MoveObj.position;
            }

            while (time < duration)
            {
                time += Time.deltaTime;
                t = time / duration;
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
