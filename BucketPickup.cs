using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketPickup : BasePickup
{
    public override void SetItemInfo()
    {
        base.SetItemInfo();
    }

    public override void PickupItem(GameObject item, Transform[] itemPostions)
    {
        base.PickupItem(item, itemPostions);

        this.transform.position = itemPostions[1].transform.position;
        this.transform.rotation = itemPostions[1].transform.rotation;
        this.transform.parent = itemPostions[1];
        playerInventory.SetAnimation("CarryingSeeds", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingSeeds", false);
    }
}
