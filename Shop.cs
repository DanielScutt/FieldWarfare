using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    Wallet playerWallet;
    GameObject player;
    Inventory inventory;
    Transform dropPoint;
    public GameObject interactText;

    public void Init(Inventory controller)
    {
        inventory = controller;
        player = controller.gameObject;
        dropPoint = GameObject.Find("DropPoint").GetComponent<Transform>();
        playerWallet = controller.GetComponent<Wallet>();
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject == player)
        {
            GameObject pickup = inventory.GetCurrentObject();
            if (pickup != null)
            {
                if(pickup.GetComponent<CropPickup>() != null)
                {
                    interactText.SetActive(true);
                }
            }
        }

        if (other.GetComponentInParent<CropPickup>())
        {
            if(!other.GetComponentInParent<Rigidbody>().isKinematic)
            {
                float price = other.GetComponentInParent<CropPickup>().GetPrice();
                Transform crop = other.GetComponentInParent<Transform>();

                DropInTruck(crop);
                playerWallet.AddToBalance(price);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactText.SetActive(false);
    }

    private void DropInTruck(Transform crop)
    {
        crop.transform.parent = dropPoint.transform;
        crop.transform.position = dropPoint.transform.position;
        Destroy(crop.gameObject.GetComponent<CropPickup>());
    }
}
