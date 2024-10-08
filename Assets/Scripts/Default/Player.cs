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
    // public Shuriken Shuriken;
    public List<CinemachineVirtualCamera> cameras;
    int currentCameraIndex = 0;
    int OldCameraIndex = -1;
    CinemachineVirtualCamera currentCamera;
    PlayerState State = PlayerState.None;
    public ItemData CurrentShurikenData;


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
        ChangeState(PlayerState.Wait);
    }
    Action UpdateAction = null;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartThrow(true);
        }
        UpdateAction?.Invoke();
    }
    private void FixedUpdate()
    {
        Movement.PlayerControl();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<ICollisionAction>() != null)
        {
            other.gameObject.GetComponent<ICollisionAction>().CollisionAction(this);
        }
        else if (other.gameObject.CompareTag("Respawn"))
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        StartCoroutine(LocalCoroutine());
        IEnumerator LocalCoroutine()
        {
            yield return new WaitForSeconds(1);
            transform.position = Z.LS.LastInstLvl.PlayerBeingTile.start.position + Vector3.up;
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<ICollisionAction>() != null)
        {
            other.gameObject.GetComponent<ICollisionAction>().CollisionAction(this);
        }
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
            Shuriken spear = Instantiate(CurrentShurikenData.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
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
        Movement.SetSpeed(0);
        animationController.Die();
    }

    private void OnGamePlay(object sender, EventArgs e)
    {
        // Movement.SetSpeed(1);
        // Movement.SetControlAble(true); 
        Movement.SetControlType(ZControlType.TwoSide);
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
    public void ChangeState(PlayerState _state)
    {
        if (State != _state)
        {
            if (_state == PlayerState.Wait)
            {
                StartThrow(false);
                ChangeCamera(0);
                Movement.UseParentedMovement(false);
                // Movement.SetControlAble(false);
                Movement.SetControlType(ZControlType.None);
                UpdateAction = null;
            }
            else if (_state == PlayerState.Obs)
            {
                StartThrow(false);
                ChangeCamera(1);
                Movement.UseParentedMovement(false);
                // Movement.SetControlAble(true);
                Movement.SetControlType(ZControlType.TwoSide);
                UpdateAction = null;
            }
            else if (_state == PlayerState.Fight)
            {
                StartThrow(true);
                ChangeCamera(2);
                Movement.UseParentedMovement(true);
                // Movement.SetControlAble(true);
                Movement.SetControlType(ZControlType.FourSide);
                Movement.ChildModelRotZero();
                UpdateAction = null;
            }
            else if (_state == PlayerState.Collect)
            {
                StartThrow(false);
                ChangeCamera(1);
                Movement.UseParentedMovement(true);
                // Movement.SetControlAble(true);
                Movement.SetControlType(ZControlType.TwoSide);
                UpdateAction = CollectUpdate;
            }
            State = _state;
        }
    }
    public PlayerState GetState()
    {
        return State;
    }
    public void CollectUpdate()
    {
        if (Physics.SphereCast(transform.position + Vector3.up, 3, Vector3.forward, out RaycastHit hit, 30f, 1 << 6))
        {
            StartThrow(true);
        }
        else
        {
            StartThrow(false);
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
        ChangeState(PlayerState.Wait);
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

public enum PlayerState
{
    None, Wait, Obs, Fight,
    Collect
}
