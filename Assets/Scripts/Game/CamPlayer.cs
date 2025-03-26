using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayer : MonoBehaviour, IItemEquipper
{
    [SerializeField] AnimationController animationController;


    [SerializeField] Transform rightFoot;
    [SerializeField] Transform leftFoot;
    [SerializeField] Transform head;
    [SerializeField] Transform chest;

    // Start is called before the first frame update
    void Start()
    {
        animationController.SetSpeed(1);
        PlayerBuffItems.OnPlayerEquipItem += WearAndShowItem;

    }

    public void WearAndShowItem(BaseItemUI item)
    {
        item.EquipItem(this);
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
}
