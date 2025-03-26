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

public class Player : Character, IItemEquipper
{
    public PlayerMovement Movement;
    [SerializeField] AnimationController animationController;
    ObjectPool<Shuriken> Pool;
    CameraController cameraController;
    SoundManager soundManager;
    UIBar bar;
    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform head;
    [SerializeField] Transform chest;
    // public Shuriken Shuriken;
    public List<CinemachineVirtualCamera> cameras;
    int currentCameraIndex = 0;
    int OldCameraIndex = -1;
    CinemachineVirtualCamera currentCamera;
    PlayerState State = PlayerState.None;
    public ItemData DefaultShuriken;

    #region BearintItems
    private Dictionary<WhereSlot, EquipData> equippedItems = new Dictionary<WhereSlot, EquipData>();

    public void EquipItem(BaseItemUI item)
    {
        if (item == null) return;

        WhereSlot slot = item.data.Where;

        // Unequip the existing item if there's one
        if (equippedItems.TryGetValue(slot, out var oldEntry))
        {
            UnequipItem(oldEntry.item);
        }

        // Instantiate and equip the new item
        EquipData data = new EquipData();
        data.item = item;
        data.InstantiatedObjects = item.InstantiateNeededItem(this);
        equippedItems[slot] = data;

        Debug.Log($"Equipped {item.data.name} in {slot}");
    }

    public void UnequipItem(BaseItemUI item)
    {
        if (item == null) return;

        WhereSlot slot = item.data.Where;
        if (equippedItems.TryGetValue(slot, out var entry))
        {
            equippedItems.Remove(slot);

            // Destroy instantiated objects

            foreach (var obj in entry.InstantiatedObjects)
            {
                Destroy(obj);
            }


            Debug.Log($"Unequipped {item.data.name} from {slot}");
        }
    }

    public BaseItemUI GetEquippedItem(WhereSlot slot)
    {
        return equippedItems.TryGetValue(slot, out var entry) ? entry.item : null;
    }
    #endregion

    public EventHandler<PlayerState> OnStateChanged;
    public Coin CoinPF;
    public Transform ForwardTransForm;
    [SerializeField] List<BaseSkill> skills;
    public EventHandler<Shuriken> OnShoot;

    // Start is called before the first frame update
    void Start()
    {
        currentCamera = cameras[currentCameraIndex];
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
        ForwardTransForm.position = new Vector3(0, 0, transform.position.z);

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
            Shuriken shuriken;

            if (GetEquippedItem(WhereSlot.Hand))
            {

                shuriken = Instantiate(GetEquippedItem(WhereSlot.Hand).data.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
                shuriken.SetInstance(GetEquippedItem(WhereSlot.Hand));
            }
            else
            {
                shuriken = Instantiate(DefaultShuriken.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
                shuriken.SetDamage();
            }
            shuriken.Pool = Pool;
            return shuriken;
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
            Shuriken spear = Instantiate(GetEquippedItem(WhereSlot.OtherHand).data.pf, transform.position, Quaternion.identity, transform.parent).GetComponent<Shuriken>();
            spear.Pool = OffHandPool;
            spear.SetInstance(GetEquippedItem(WhereSlot.OtherHand));
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
        if (GetEquippedItem(WhereSlot.OtherHand))
        {
            OffHandItemInstance offHandItemInstance = (OffHandItemInstance)GetEquippedItem(WhereSlot.OtherHand);
            offHandItemInstance.OnOffHandItem += OnOffhandItemInvoke;
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
        shuriken.OnShoot(this);

        OnShoot?.Invoke(Pool, shuriken);
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
                UpdateAction = CollectUpdate;
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
                OffHandItem = (OffHandItemInstance)GetEquippedItem(WhereSlot.OtherHand);
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
        if (Physics.SphereCast(transform.position + Vector3.up, 3, Vector3.forward, out RaycastHit hit, 100f, 1 << 6))
        {
            StartThrow(true);
        }
        else
        {
            StartThrow(false);
        }
    }
    OffHandItemInstance OffHandItem;

    public void FightUpdate()
    {
        OffHandItem?.Tick();
    }
    public void StartThrow(bool val = true)
    {
        // if (HandItem == null)
        // {
        //     return;
        // }
        animationController.RightHandAttack(val);
        // Debug.Log("Hand speed " + HandItem.data.BaseSpeed);
        ItemData data;
        if (GetEquippedItem(WhereSlot.Hand))
        {
            data = (ItemData)GetEquippedItem(WhereSlot.Hand).data;
        }
        else
        {
            data = DefaultShuriken;
        }
        animationController.SetHandSpeed(data.BaseSpeed + Stats.AttackSpeed.GetValue());
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

    internal void AddToSkill(BaseSkill skill)
    {
        if (skills.Contains(skill))
        {
            return;
        }
        skills.Add(skill);
        // skill.Equip();
    }

    internal void RemoveSkill(BaseSkill skill)
    {
        if (skills.Contains(skill))
        {
            skills.Remove(skill);
            // skill.UnEquip();
        }
    }

    public Transform GetRightFoot()
    {
        return rightFoot;
    }

    public Transform GetLeftFoot()
    {
        return leftFoot;
    }

    public Transform GetHeadTransform()
    {
        return head;
    }

    public Transform GetChest()
    {
        return chest;
    }

}

public enum PlayerState
{
    None, Wait, Obs, Fight,
    Collect
}
