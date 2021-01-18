using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropPickup : BasePickup, ICrop
{
    [SerializeField] int daysToGrow;
    [SerializeField] float price;
    [SerializeField] Mesh[] growth;
    [SerializeField] MeshCollider meshColl;

    [SerializeField] int currentDay;
    int stageOne;
    int stageTwo;
    int stageThree;

    bool grown = false;
    bool watered = false;


    private void Awake()
    {
        //Set the mesh and mesh collider to the first stage.
        GetComponentInChildren<MeshFilter>().mesh = growth[0];
    }

    private void Start()
    {
        currentDay = 0;

        //Cal the different stage depending on how long it takes to grow
        stageOne = daysToGrow / 3;
        stageTwo = (daysToGrow / 3) * 2;
        stageThree = (daysToGrow / 3) * 3;

        base.SetCanPickup(false);
    }

    //Calls every time a day pasts
    public void Grow()
    {
        if (!grown)
        {
            currentDay++;
            watered = false;

            if (currentDay <= stageOne)
            {
                GetComponentInChildren<MeshFilter>().mesh = growth[0];
                meshColl.sharedMesh = growth[0];
            }
            else if (currentDay <= stageTwo)
            {
                GetComponentInChildren<MeshFilter>().mesh = growth[1];
                meshColl.sharedMesh = growth[1];
            }
            else if (currentDay < stageThree)
            {
                GetComponentInChildren<MeshFilter>().mesh = growth[2];
                meshColl.sharedMesh = growth[2];
            }
            else if (currentDay == daysToGrow)
            {
                GetComponentInChildren<MeshFilter>().mesh = growth[3];
                meshColl.sharedMesh = growth[3];
            }

            if (currentDay >= daysToGrow)
            {
                grown = true;
                watered = true;
            }
        }
    }

    public float GetPrice()
    {
        return price;
    }

    public bool GetGrown()
    {
        return grown;
    }

    public override void PickupItem(GameObject item, Transform[] itemPostions)
    {
        base.PickupItem(item, itemPostions);

        this.transform.position = itemPostions[1].transform.position;
        this.transform.rotation = itemPostions[1].transform.rotation;
        this.transform.parent = itemPostions[1];
        playerInventory.SetAnimation("CarryingSeeds", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingSeeds", false);
    }

    public bool NeedsWater()
    {
        return !watered;
    }
    public void CropWatered()
    {
        watered = true;
    }
}
