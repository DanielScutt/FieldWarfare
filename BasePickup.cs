using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePickup : MonoBehaviour, IPickup
{
    protected Rigidbody rb;
    protected Inventory playerInventory;

    public bool canPickup = true;
    public AudioSource drop;

    protected struct ItemInfo
    {
        public string itemName;
        public string itemAmount;
        public string itemControls;
    }

    protected ItemInfo itemInfo = new ItemInfo();

    public virtual void Init(Inventory playerInven)
    {
        rb = GetComponent<Rigidbody>();
        playerInventory = playerInven;
        SetItemInfo();
    }

    public virtual void SetItemInfo()
    {
        itemInfo.itemName = "Test";
        itemInfo.itemAmount = "null";
        itemInfo.itemControls = "null";
    }

    public string GetItemName()
    {
        return itemInfo.itemName;
    }

    public string GetItemAmount()
    {
        return itemInfo.itemAmount.ToString();
    }

    public string GetItemControls()
    {
        return itemInfo.itemControls;
    }

    //Allows you to chnage if the pickup can be picked up by the player.
    public void SetCanPickup(bool CanPickup)
    {
        canPickup = CanPickup;
    }

    public virtual void PickupItem(GameObject item, Transform[] itemPositions)
    {
        //Checks if the player can pickup the item
        if(canPickup)
        {
            //Parent the item to the player and set position to zero
            //this.transform.parent = rightHand;
            //this.transform.rotation = rightHand.transform.rotation;
            //this.transform.localPosition = Vector3.zero;

            //Turn off all rb effects
            rb.detectCollisions = false;
            rb.isKinematic = true;

            drop.Play();
        }
    }

    public virtual void DropItem()
    {
        //Unparent the item
        this.transform.parent = null;

        //Turn on all rb effects
        rb.detectCollisions = true;
        rb.isKinematic = false;
    }

    public bool GetCanPickup()
    {
        return canPickup;
    }

    public void ChangeItemPosition(Transform pos)
    {
        this.transform.position = pos.transform.position;
        this.transform.rotation = pos.transform.rotation;
        this.transform.parent = pos.transform;
    }
}
