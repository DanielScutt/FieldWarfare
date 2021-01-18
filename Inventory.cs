using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Transform[] itemPositions;

    InputMaster input;
    IPickup pickingItem;
    GameObject pickingObject;

    IPickup currentItem;
    GameObject currentObject;
    bool hasItem;
    bool fieldTut, turretTut;

    Animator animator;

    public void Init()
    {
        animator = GetComponent<Animator>();
        input = new InputMaster();
        input.Enable();

        input.Player.Equip.performed += _ => PickupItem();
        input.Player.Drop.performed += _ => DropItem();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (hasItem) { return; }
        BasePickup pickup = other.GetComponent<BasePickup>();
        if(pickup != null)
        {
            IPickup isPickup = pickup as IPickup;
            if(isPickup != null)
            {
                pickingItem = isPickup;
                pickingObject = other.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (hasItem) { return; }
        if (other.gameObject == pickingObject)
        {
            pickingItem = null;
            pickingObject = null;
        }
    }

    private void PickupItem()
    {
        if (hasItem) { return; }
        if (pickingItem != null)
        {
            if(pickingItem.GetCanPickup())
            {
                pickingItem.PickupItem(pickingObject, itemPositions);
                SetHasItem(true, pickingItem, pickingObject);
            }
        }
    }

    public void ChangeItem(GameObject item, IPickup pickup)
    {
        pickingItem = pickup;
        if (pickingItem != null)
        {
            if (pickingItem.GetCanPickup())
            {
                pickingItem.PickupItem(item, itemPositions);
                SetHasItem(true, pickup, item);
            }
        }
    }

    public void DropItem()
    {
        if (!hasItem) { return; }
        currentItem.DropItem();
        SetHasItem(false, null, null);
    }

    public void SetHasItem(bool HasItem, IPickup item, GameObject itemObject)
    {
        hasItem = HasItem;
        currentItem = item;
        currentObject = itemObject;
        pickingItem = null;
        pickingItem = null;
    }

    public bool GetHasItem()
    {
        return hasItem;
    }

    public IPickup GetCurrentItem()
    {
        return currentItem;
    }

    public GameObject GetCurrentObject()
    {
        return currentObject;
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public void SetAnimation(string animName, bool state)
    {
        animator.SetBool(animName, state);
    }

    public bool IsInAnimation()
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;
    }

    public void DisableInput()
    {
        input.Disable();
    }

    public void EnableInput()
    {
        input.Enable();
    }

    public void PutItemOnBack()
    {
        currentItem.ChangeItemPosition(itemPositions[3]);
    }

    public void PutItemOnFront()
    {
        if(currentItem != null)
        {
            currentItem.ChangeItemPosition(itemPositions[1]);
        }
    }

    public bool GetFieldTut()
    {
        return fieldTut;
    }

    public void SetFieldTut()
    {
        fieldTut = true;
    }

    public bool GetTurretTut()
    {
        return turretTut;
    }

    public void SetTurretTut()
    {
        turretTut = true;
    }
}
