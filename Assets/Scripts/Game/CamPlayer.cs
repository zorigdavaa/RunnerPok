using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayer : MonoBehaviour, IItemEquipper
{
    [SerializeField] AnimationController animationController;

    public ItemInstance HandItem { get; set; }
    public OffHandItemInstance OffHandItem { get; set; }
    public HeadItemInstance HeadItem { get; set; }
    public FootItemInstance FootItem { get; set; }
    public ChestItemInstance ChestItem { get; set; }
    public ItemInstance Necklace { get; set; }

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
}
