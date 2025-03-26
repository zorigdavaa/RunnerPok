using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemEquipper
{
    public ItemInstance HandItem { get; set; }
    public OffHandItemInstance OffHandItem { get; set; }
    public HeadItemInstance HeadItem { get; set; }
    public FootItemInstance FootItem { get; set; }
    public ChestItemInstance ChestItem { get; set; }
    public ItemInstance Necklace { get; set; }
    public Transform GetRightFoot();
    public Transform GetLeftFoot();
    public Transform GetHeadTransform();
    public Transform GetChest();

}