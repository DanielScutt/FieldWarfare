using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    const string ITEM_POSITION = "Item Position";

    [SerializeField] bool canPickUp = true;
    [SerializeField] Rigidbody rb;

    bool pickedUp = false;

    BoxCollider player;
    Transform plantPosition;
    PlayerController playerController;


    private void Awake()
    {
        player = GameObject.FindObjectOfType<PlayerController>().GetComponentInChildren<BoxCollider>();
        playerController = GameObject.FindObjectOfType<PlayerController>().GetComponent<PlayerController>();
        plantPosition = GameObject.Find(ITEM_POSITION).GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
    }

    private void Pickup()
    {
        this.transform.parent = null;
        this.transform.parent = plantPosition.transform;
        this.transform.rotation = plantPosition.transform.rotation;
        rb.detectCollisions = false;
        rb.isKinematic = true;
        this.transform.localPosition = Vector3.zero;
        pickedUp = true;
    }

    public void Drop()
    {
        transform.parent = null;
        rb.isKinematic = false;
        rb.detectCollisions = true;
        pickedUp = false;
    }

    public void SetCanPickUp(bool ableToPickUp)
    {
        canPickUp = ableToPickUp;
    }

    public bool PickedUp()
    {
        return pickedUp;
    }
}
