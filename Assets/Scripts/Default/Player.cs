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
    public Transform rightFoot;
    public Transform leftFoot;
    public Transform head;
    public Transform chest;
    // public Shuriken Shuriken;
    public List<CinemachineVirtualCamera> cameras;
    int currentCameraIndex = 0;
    int OldCameraIndex = -1;
    CinemachineVirtualCamera currentCamera;
    PlayerState State = PlayerState.None;

    #region BearintItems
    private ItemData _handItem;
    public ItemData HandItem
    {
        get { return _handItem; }
        set { _handItem = value; }
    }
    private OffHandItem _offHandItem;
    public OffHandItem OffHandItem
    {
        get { return _offHandItem; }
        set { _offHandItem = value; }
    }
    private ItemData _headItem;
    public ItemData HeadItem
    {
        get { return _headItem; }
        set
        {
            _headItem = value;
            if (_headItem != null)
            {
                _headItem.Wear(this);
            }
        }
    }
    public GameObject RightFootObj;
    public GameObject LeftFootObj;
    private FootItemdata _FootItem;
    public FootItemdata FootItem
    {
        get { return _FootItem; }
        set
        {
            if (_FootItem != value)
            {
                if (value != null)
                {
                    value.Wear(this);
                }
                else
                {
                    if (_FootItem)
                    {
                        _FootItem.Unwear(this);
                    }
                }
            }
            _FootItem = value;
        }
    }
    private ChestItemData _chestItem;
    public ChestItemData ChestItem
    {
        get { return _chestItem; }
        set
        {
            _chestItem = value;
            if (_chestItem != null)
            {
                _chestItem.Wear(this);
            }
        }
    }
    #endregion

    public EventHandler<PlayerState> OnStateChanged;
    public Coin CoinPF;


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
        // if (Input.GetMouseButtonUp(0))
        // {
        //     if (GetState() == PlayerState.Obs && Movement.IsGrounded())
        //     {
        //         Movement.Jump();
        //     }
        // }
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
    private void InitHandPool()
    {
        Pool = new ObjectPool<Shuriken>(() =>
        {
            Shuriken spear = Instantiate(HandItem.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
            spear.Pool = Pool;
            return spear;
            // return new GameObject();
        }, (s) =>
        {
            s.transform.position = transform.position + new Vector3(0, 1, 0);
            s.GetFrompool();
        }, (s) =>
        {
            // release
            s.GotoPool();
        });
    }
    ObjectPool<Shuriken> OffHandPool;
    private void InitOffHandPool()
    {
        OffHandPool = new ObjectPool<Shuriken>(() =>
        {
            Shuriken spear = Instantiate(OffHandItem.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
            spear.Pool = OffHandPool;
            return spear;
            // return new GameObject();
        }, (s) =>
        {
            s.transform.position = transform.position + new Vector3(0, 1, 0);
            s.GetFrompool();
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
        SubscribeWeaponEvents();
        InitHandPool();
        InitOffHandPool();
    }

    private void SubscribeWeaponEvents()
    {
        if (OffHandItem)
        {
            OffHandItem.OnOffHandItem += OnOffhandItemInvoke;
        }
    }

    private void OnOffhandItemInvoke(object sender, EventArgs e)
    {
        // Instantiate(OffHandItem.pf, transform.position, Quaternion.identity);
        OffHandPool.Get();
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
                UpdateAction = FightUpdate;
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
            OnStateChanged?.Invoke(this, State);
        }
    }
    public PlayerState GetState()
    {
        return State;
    }
    public void CollectUpdate()
    {
        if (Physics.SphereCast(transform.position + Vector3.up, 3, Vector3.forward, out RaycastHit hit, 50f, 1 << 6))
        {
            StartThrow(true);
        }
        else
        {
            StartThrow(false);
        }
    }
    public void FightUpdate()
    {
        OffHandItem?.Update();
    }
    public void StartThrow(bool val = true)
    {
        if (HandItem)
        {
            animationController.RightHandAttack(val);
            Debug.Log("Hand speed " + HandItem.BaseSpeed);
            animationController.SetHandSpeed(HandItem.BaseSpeed);
        }
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

    internal void TakeCoin(Transform posTransform)
    {
        Coin coin = Instantiate(CoinPF, posTransform.position + Vector3.up, Quaternion.identity);
        coin.GotoPosAndAdd();
    }
}

public enum PlayerState
{
    None, Wait, Obs, Fight,
    Collect
}
