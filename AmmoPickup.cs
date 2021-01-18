using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : BasePickup, IAmmo
{
    public int ammo;
    public TextMesh ammoText;

    public override void Init(Inventory playerInven)
    {
        base.Init(playerInven);
        ammoText.text = ammo.ToString();
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

    public override void SetItemInfo()
    {
        base.SetItemInfo();
    }

    public void UsedAmmo(int remainingAmmo)
    {
        ammo = remainingAmmo;
        if(ammo <= 0)
        {
            playerInventory.DropItem();
            this.SetCanPickup(false);
            Destroy(this.gameObject, 3);
        }

        ammoText.text = ammo.ToString();
    }

    public int GetAmmo()
    {
        return ammo;
    }
}
