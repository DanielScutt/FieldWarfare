using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelPickup : BasePickup
{
    // Start is called before the first frame update
    void Start()
    {
        itemInfo.itemAmount = "";
        itemInfo.itemControls = "Press F to Plant Crops\nPress Q to Drop";
    }

    public override void PickupItem(GameObject item, Transform[] itemPostions)
    {
        base.PickupItem(item, itemPostions);

        this.transform.position = itemPostions[2].transform.position;
        this.transform.rotation = itemPostions[2].transform.rotation;
        this.transform.parent = itemPostions[2];
        playerInventory.SetAnimation("CarryingTool", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingTool", false);
    }
}
