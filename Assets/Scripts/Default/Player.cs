using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZPackage;
using ZPackage.Helper;
using Random = UnityEngine.Random;
using UnityEngine.Pool;
using ZPackage.Utility;
using System.Linq;

public class Player : Character
{
    ObjectPool<GameObject> Pool;
    CameraController cameraController;
    SoundManager soundManager;
    UIBar bar;
    public AnimationController animController;

    // Start is called before the first frame update
    void Start()
    {

        movement = GetComponent<MovementForgeRun>();
        // animationController.OnSpearShoot += SpearShoot;
        soundManager = FindObjectOfType<SoundManager>();
        // cameraController = FindObjectOfType<CameraController>();
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.GamePlay += OnGamePlay;
        GameManager.Instance.LevelCompleted += OnGameOver;
        InitPool();
        GameManager.Instance.Coin = 10;
    }
    public Vector2 FindNearestCenterOffset(List<Vector2> ToFindPoints)
    {
        Vector2 nearestPoint = ToFindPoints[0];
        Vector2 center = new Vector2(2,2);
        float nearestDistance = Vector2.Distance(center, nearestPoint);

        foreach (Vector2 point in ToFindPoints)
        {
            float distance = Vector2.Distance(center, point);
            if (distance < nearestDistance)
            {
                nearestPoint = point;
                nearestDistance = distance;
            }
        }

        return center -  nearestPoint;
    }



    private void Update()
    {
    }

    private void FindNearestEnemy()
    {
        float shortest = 100;
        Transform nearest = null;
        foreach (var item in Physics.OverlapSphere(transform.position, 10, 1 << 3))
        {
            float Distance = Vector3.Distance(transform.position, item.transform.position);
            if (shortest > Distance)
            {
                nearest = item.transform;
                shortest = Distance;
            }
        }
    }

    private void InitPool()
    {
        Pool = new ObjectPool<GameObject>(() =>
        {
            // GameObject spear = Instantiate(Spear, Vector3.zero, Spear.transform.rotation);
            // spear.SetPool(Pool);
            // return spear;
            return new GameObject();
        }, (s) =>
        {
            // s.transform.position = Spear.transform.position;
            // // s.transform.rotation = Spear.transform.rotation;
            // if (Target)
            // {
            //     s.Throw(Target);
            // }
            // else
            // {
            //     s.Throw(transform.forward);
            // }
        }, (s) =>
        {
            //release
            // s.GotoPool();
        });
    }
    public void Die()
    {
        GameManager.Instance.GameOver(this, EventArgs.Empty);
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        movement.SetSpeed(1);
        movement.SetControlAble(true);
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        // throw new NotImplementedException();
    }

    internal void DoneBoard()
    {
        throw new NotImplementedException();
    }
}
