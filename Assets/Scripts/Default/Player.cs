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
        currentCamera = cameras[currentCameraIndex];
        Health = MaxHealth;
        Movement = GetComponent<PlayerMovement>();
        Movement.SetSpeed(1);
        // animationController.OnSpearShoot += SpearShoot;
        soundManager = FindObjectOfType<SoundManager>();
        // cameraController = FindObjectOfType<CameraController>();
        GameManager.Instance.GameOverEvent += OnGameOver;
        GameManager.Instance.OnGamePlay += OnGamePlay;
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
        // Movement.SetSpeed(1);
        Movement.SetControlAble(true);
    }
    private void AttackProjectile(object sender, EventArgs e)
    {
        Shuriken shuriken = Pool.Get();
        if (Physics.SphereCast(transform.position + Vector3.up, 2f, transform.forward, out RaycastHit hit, 20, 1 << 6))
        {
            Vector3 point = new Vector3(transform.position.x, hit.transform.position.y, hit.transform.position.z);
            float distance = Vector3.Distance(point, hit.transform.position);

            Vector3 dirToNearest = transform.position - hit.transform.position;
            float half = distance * 0.7f;
            if (Vector3.Dot(dirToNearest, transform.right) > 0) // right

            {
                shuriken.SideMovement = -half;
            }
            else
            {
                shuriken.SideMovement = half;
            }


            // Collider[] enemies = Physics.OverlapSphere(transform.position + transform.forward * 20, 2f, 1 << 6);
            // float nearestDistance = Mathf.Infinity; // Initialize with a very large value
            // Transform nearestEnemy = null;

            // foreach (var enemy in enemies)
            // {
            //     Vector3 point = new Vector3(transform.position.x, enemy.transform.position.y, enemy.transform.position.z);
            //     // Calculate the distance between the shuriken and the current enemy
            //     float distance = Vector3.Distance(point, enemy.transform.position);

            //     // Check if the current enemy is closer than the nearest one found so far
            //     if (distance < nearestDistance)
            //     {
            //         nearestDistance = distance;
            //         nearestEnemy = enemy.transform;
            //     }
            // }
            // float distance = Vector3.Distance(point, enemy.transform.position);
            // if (nearestEnemy != null)
            // {
            //     Vector3 dirToNearest = transform.position - nearestEnemy.position;
            //     float half = nearestDistance * 0.7f;
            //     if (Vector3.Dot(dirToNearest, transform.right) > 0) // right

            //     {
            //         shuriken.SideMovement = -half;
            //     }
            //     else
            //     {
            //         shuriken.SideMovement = half;
            //     }

            // }
        }

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

    internal void GoingToFight(bool v)
    {
        Movement.UseParentedMovement(v);
        if (v)
        {
            StartThrow(true);
            ChangeCamera(1);
        }
        else
        {
            StartThrow(false);
            ChangeCamera(0);
        }
    }
}
