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
using Cinemachine;

public class Player : Character
{
    public PlayerMovement Movement;
    [SerializeField] AnimationController animationController;
    ObjectPool<Shuriken> Pool;
    CameraController cameraController;
    SoundManager soundManager;
    UIBar bar;
    public AnimationController animController;
    public Shuriken Shuriken;
    public List<CinemachineVirtualCamera> cameras;
    int currentCameraIndex = 0;
    int OldCameraIndex = -1;
    CinemachineVirtualCamera currentCamera;


    // Start is called before the first frame update
    void Start()
    {
        Health = MaxHealth;
        Movement = GetComponent<PlayerMovement>();
        // animationController.OnSpearShoot += SpearShoot;
        soundManager = FindObjectOfType<SoundManager>();
        // cameraController = FindObjectOfType<CameraController>();
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.GamePlay += OnGamePlay;
        GameManager.Instance.LevelCompleted += OnGameOver;
        InitPool();
        GameManager.Instance.Coin = 10;
        animationController.OnAttackEvent += AttackProjectile;
    }



    public Vector2 FindNearestCenterOffset(List<Vector2> ToFindPoints)
    {
        Vector2 nearestPoint = ToFindPoints[0];
        Vector2 center = new Vector2(2, 2);
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

        return center - nearestPoint;
    }


    public bool UseAttack = false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartThrow(true);
        }
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
    public void ChangeCamera(int index)
    {
        OldCameraIndex = currentCameraIndex;
        if (currentCameraIndex != index)
        {
            currentCamera.Priority = 0;
            currentCamera = cameras[index];
            currentCamera.Priority = 1;
            currentCameraIndex = index;

        }
    }

    private void InitPool()
    {
        Pool = new ObjectPool<Shuriken>(() =>
        {
            Shuriken spear = Instantiate(Shuriken, transform.position, Quaternion.identity, transform.parent);
            spear.SetPool(Pool);
            return spear;
            // return new GameObject();
        }, (s) =>
        {
            s.transform.position = transform.position + new Vector3(0, 1, 0);
            s.GetFrompool();
            // s.transform.rotation = Spear.transform.rotation;
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
            // release
            s.GotoPool();
        });
    }
    public override void Die()
    {
        GameManager.Instance.GameOver(this, EventArgs.Empty);
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        Movement.SetSpeed(1);
        Movement.SetControlAble(true);
    }
    private void AttackProjectile(object sender, EventArgs e)
    {
        Pool.Get();
    }
    public void StartThrow(bool val = true)
    {
        animationController.RightHandAttack(val);
        // attackTimer -= Time.deltaTime;
        // if (attackTimer <= 0)
        // {
        //     animationController.Attack();
        //     attackTimer = InitialAttackTimer;
        //     // Shuriken = Instantiate(Shuriken, transform.position, Quaternion.identity, transform.parent);

        //     // Destroy(Shuriken.gameObject, 5);
        // }
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
