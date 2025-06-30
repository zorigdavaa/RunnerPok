using UnityEngine;
using UnityEngine.Events;
using ZPackage;

public class PlayerFarAnimation : MonoBehaviour
{
    Player player;
    public float PlayerNearDistance = 20;
    public bool DestroySelf = true;
    bool Detected = false;
    public UnityEvent WhenNear;
    public UnityEvent FarAction;
    public UnityEvent StartAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = Z.Player;
        StartAction?.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z - PlayerNearDistance > player.transform.position.z)
        {
            Debug.Log("Far");
            FarAction?.Invoke();
        }
        else
        {
            Debug.Log("Near");
            if (!Detected)
            {
                WhenNear?.Invoke();
                Detected = true;
                if (DestroySelf)
                {
                    Destroy(this);
                }
            }
        }
    }
    public void ChasePlayerX()
    {
        transform.position = transform.position.ChangeX(player.transform.position.x);
    }
}
