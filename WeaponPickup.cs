using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : BasePickup, IWeapon
{
    public float fireRate = 0.5f;
    public int maxAmmo = 10;
    public int ammoInClip = 2;
    int currentAmmo;
    bool fired = false;

    private void Start()
    {
        currentAmmo = ammoInClip;
        itemInfo.itemName = "Shotgun";
        itemInfo.itemAmount = currentAmmo.ToString() + " / " + maxAmmo.ToString();
        itemInfo.itemControls = "Hold Left Click to Fire\nPress R to Reload\nPress Q to Drop";
    }

    public void Fire()
    {
        if (currentAmmo > 0)
        {
            if (!fired)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, this.transform.forward * 100, out hit, 11))
                {
                    MutantController mutant = hit.collider.gameObject.GetComponent<MutantController>();
                    if(mutant != null)
                    {
                        mutant.TakeDamage(100);
                    }
                }
                Debug.DrawLine(this.transform.position, this.transform.forward * 100, Color.white, 2.5f);
                fired = true;
                currentAmmo--;
                StartCoroutine(FireRate());
                itemInfo.itemAmount = currentAmmo.ToString() + " / " + maxAmmo.ToString();
            }
        }
    }

    public void Reload()
    {
        if(currentAmmo != ammoInClip)
        {
            maxAmmo += currentAmmo;

            if(maxAmmo > 0)
            {
                if(maxAmmo < ammoInClip)
                {
                    currentAmmo = maxAmmo;
                    maxAmmo = 0;
                }
                else
                {
                    maxAmmo -= ammoInClip;
                    currentAmmo = ammoInClip;
                }
            }
        }
        itemInfo.itemAmount = currentAmmo.ToString() + " / " + maxAmmo.ToString();
    }

    IEnumerator FireRate()
    {
        yield return new WaitForSeconds(fireRate);
        fired = false;
        StopCoroutine(FireRate());
    }

    public override void PickupItem(GameObject item, Transform[] itemPostions)
    {
        base.PickupItem(item, itemPostions);

        this.transform.position = itemPostions[2].transform.position;
        this.transform.rotation = itemPostions[2].transform.rotation;
        this.transform.parent = itemPostions[2];
        playerInventory.SetAnimation("CarryingGun", true);

        //Parent the item to the player and set position to zero
        //topHandle.transform.rotation = leftHand.transform.rotation;


        //Parent the item to the player and set position to zero
        //bottomHandle.transform.parent = rightHand;
        //bottomHandle.transform.rotation = rightHand.transform.rotation;
        //bottomHandle.transform.localPosition = Vector3.zero;
    }
}
