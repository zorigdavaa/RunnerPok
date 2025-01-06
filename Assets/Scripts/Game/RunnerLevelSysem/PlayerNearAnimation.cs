using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ZPackage;
#if UNITY_EDITOR
using UnityEditor.Events;
#endif
public class PlayerNearAnimation : MonoBehaviour
{
    Player player;
    public float PlayerNearDistance = 20;
    public bool DestroySelf = true;
    public UnityEvent WhenNear;
    bool Detected = false;
    // Start is called before the first frame update
    void Start()
    {
        player = Z.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.z > transform.position.z - PlayerNearDistance && !Detected)
        {
            Detected = true;
            WhenNear?.Invoke();
            if (DestroySelf)
            {
                Destroy(this);
            }
        }
    }
#if UNITY_EDITOR
    [ContextMenu("GetTransformAnimation")]
    public void GetTransformAnimation()
    {
        // Remove all existing persistent listeners
        for (int i = WhenNear.GetPersistentEventCount() - 1; i >= 0; i--)
        {
            UnityEventTools.RemovePersistentListener(WhenNear, i);
        }

        TransformAnimation transformAnimation = GetComponent<TransformAnimation>();
        // UnityAction newAction = () =>
        // {
        //     transformAnimation.Animate();
        // };
        UnityEventTools.AddVoidPersistentListener(WhenNear, transformAnimation.Animate);
        // WhenNear.AddListener(newAction);
    }
#endif
}
