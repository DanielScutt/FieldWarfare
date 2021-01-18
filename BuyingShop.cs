using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyingShop : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject[] items;
    public Transform itemSpawn;
    int itemPrice;
    Wallet playerWallet;
    Inventory playerInventory;
    PlayerMovement playerMovement;
    public GameObject interactText;
    public GameObject[] fields;
    int fieldCount;

    public virtual void Init(Inventory inventory, PlayerMovement movement)
    {
        playerInventory = inventory;
        playerMovement = movement;
        playerWallet = inventory.gameObject.GetComponent<Wallet>();
        foreach(GameObject field in fields)
        {
            field.GetComponent<SimpleField>().Locked();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == playerInventory.gameObject)
        {
            interactText.SetActive(true);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == playerInventory.gameObject)
        {
            interactText.SetActive(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject == playerInventory.gameObject)
        {
            if (Input.GetButton("Interact"))
            {
                shopUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                playerMovement.DisableMovement();
                playerInventory.DisableInput();
                playerMovement.DisableCameraControls();
            }
        }
    }
    public void BuyAmmo()
    {
        itemPrice = 50;
        SpawnItem(items[8], itemPrice);
    }

    public void BuyField()
    {
        if(fieldCount != (fields.Length))
        {
            fields[fieldCount].SetActive(true);
            fieldCount++;
            itemPrice = 1000;
            SpawnItem(null, itemPrice);
        }
    }

    public void BuyDamageUpgrade()
    {
        itemPrice = 300;
        SpawnItem(items[3], itemPrice);
    }


    public void BuyFireRateUpgrade()
    {
        itemPrice = 200;
        SpawnItem(items[4], itemPrice);
    }

    public void BuyBeetrootSeeds()
    {
        itemPrice = 150;
        SpawnItem(items[6], itemPrice);
    }

    public void BuyLettuceSeeds()
    {
        itemPrice = 200;
        SpawnItem(items[5], itemPrice);
    }

    public void BuyTurret()
    {
        itemPrice = 500;
        SpawnItem(items[0], itemPrice);
    }

    public void BuyMelonSeed()
    {
        itemPrice = 200;
        SpawnItem(items[1], itemPrice);
    }

    public void BuyPumpkinSeed()
    {
        itemPrice = 250;
        SpawnItem(items[2], itemPrice);
    }

    public void BuyCucumberSeed()
    {
        itemPrice = 150;
        SpawnItem(items[7], itemPrice);
    }

    void SpawnItem(GameObject item, int price)
    {
        if(playerWallet.GetBalance() >= price)
        {
            playerWallet.MinusBalance(price);
            if (item != null)
            {
                BasePickup spawnedItem = Instantiate(item, itemSpawn.position, Quaternion.identity).GetComponent<BasePickup>();
                spawnedItem.Init(playerInventory);
            }
        }
    }

    public void ExitShop()
    {
        shopUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerMovement.EnableMovement();
        playerInventory.EnableInput();
        playerMovement.EnableCameraControls();
    }
}
