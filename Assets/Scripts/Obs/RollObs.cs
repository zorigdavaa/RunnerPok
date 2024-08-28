using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;

public class RollObs : MonoBehaviour, ICollisionAction
{
    [SerializeField] Rigidbody Roller;
    bool isRolling = false;
    float selfTime;
    float speed = 2;
    public void CollisionAction(Character character)
    {
        if (character is Player player)
        {
            Roller.isKinematic = false;
            Roller.velocity = Vector3.back * 30;
            Roller.angularVelocity = new Vector3(10, 0, 0);
            isRolling = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Roller.transform.position += Vector3.right * Random.Range(-4, 4);
    }

    // Update is called once per frame
    void Update()
    {

        if (!isRolling)
        {
            selfTime += Time.deltaTime * speed;
            Vector3 pos = Roller.transform.position;
            pos.x = Mathf.PingPong(selfTime, 4) - 2f;
            Roller.transform.position = pos;
        }
    }
}
