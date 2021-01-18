using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum cropTypes { Pumpkin, Melon, Squash, Beetroot, Lettuce };

public class SeedPickup : BasePickup, ISeed
{
    [SerializeField] cropTypes thisCrop;
    [SerializeField] int seedAmount;

    private void Start()
    {
        if (this.thisCrop == cropTypes.Melon)
        {
            itemInfo.itemName = "Melon Seeds";
        }
        else if (this.thisCrop == cropTypes.Pumpkin)
        {
            itemInfo.itemName = "Pumpkin Seeds";
        }
        else if (this.thisCrop == cropTypes.Squash)
        {
            itemInfo.itemName = "Squash Seeds";
        }
        else if (this.thisCrop == cropTypes.Beetroot)
        {
            itemInfo.itemName = "Beetroot Seeds";
        }
        else if (this.thisCrop == cropTypes.Lettuce)
        {
            itemInfo.itemName = "Lettuce Seeds";
        }
        itemInfo.itemAmount = seedAmount.ToString();
        itemInfo.itemControls = "Press F to Plant Crops\nPress Q to Drop";
    }

    public string GetCropType()
    {
        return thisCrop.ToString();
    }

    public void UpdateSeedAmount()
    {
        seedAmount--;
        itemInfo.itemAmount = seedAmount.ToString();
        if(seedAmount < 0)
        {
            SetCanPickup(false);
            playerInventory.DropItem();
            Destroy(this.gameObject, 5f);
        }
    }

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

    public void Used()
    {
        SetCanPickup(false);
        playerInventory.DropItem();
        Destroy(this.gameObject, 5f);
    }
}
