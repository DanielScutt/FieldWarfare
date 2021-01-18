using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PloughPickup : BasePickup
{
    public BoxCollider damageTrigger;

    public override void Init(Inventory playerInven)
    {
        base.Init(playerInven);
        damageTrigger.enabled = false;
    }

    public override void SetItemInfo()
    {
        base.SetItemInfo();
    }

    public override void PickupItem(GameObject item, Transform[] itemPostions)
    {
        base.PickupItem(item, itemPostions);

        this.transform.position = itemPostions[0].transform.position;
        this.transform.rotation = itemPostions[0].transform.rotation;
        this.transform.parent = itemPostions[0];
        playerInventory.SetAnimation("CarryingTool", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingTool", false);
    }
}
