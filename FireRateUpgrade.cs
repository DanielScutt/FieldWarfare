using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateUpgrade : BasePickup, IUpgrade
{
    public override void PickupItem(GameObject item, Transform[] itemPositions)
    {
        base.PickupItem(item, itemPositions);
        this.transform.position = itemPositions[1].transform.position;
        this.transform.rotation = itemPositions[1].transform.rotation;
        this.transform.parent = itemPositions[1];
        playerInventory.SetAnimation("CarryingSeeds", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingSeeds", false);
    }

    public override void SetItemInfo()
    {
        base.SetItemInfo();
    }

    public float UpgradeStat(float currentStat)
    {
        float newStat = currentStat / 5;

        currentStat -= newStat;

        this.SetCanPickup(false);
        playerInventory.DropItem();

        Destroy(this.gameObject, 3f);

        return currentStat;
    }
}
