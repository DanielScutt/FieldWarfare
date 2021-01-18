using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour
{
    enum FieldStates { Plough, Plant, Water, Growing, NeedsWater, Grown }

    FieldStates currentState;

    Coroutine coroutine;

    bool inField = false;
    bool needsPlough = true;
    bool canPlant = false;
    bool needWater = false;
    bool watered = false;
    bool growing = false;
    bool ploughing = false;
    bool planting = false;
    bool picking = false;
    bool watering = false;

    Inventory playerInventory;
    PlayerMovement movement;
    CropManager cropManager;
    GameObject player;
    GameObject cropToWater;
    GameObject cropToPick;

    public GameObject intereactText;
    public Mesh[] meshes;
    public GameObject[] cropPos;
    public GameObject[] cropList;
    public List<GameObject> placedCrops;
    public List<GameObject> removeCrops;
    GameObject currentPos;
    GameObject currentCrop;

    float currentPloughTime;
    float ploughTime;
    float conPloughTime;

    float currentWaterTime;
    float waterTime;
    float conWaterTime;

    public void Init(Inventory inventory, CropManager cropManagerRef, PlayerMovement playerMovement)
    {
        cropManager = cropManagerRef;
        cropManager.AddField(this.gameObject);
        playerInventory = inventory;
        currentState = FieldStates.Plough;
        movement = playerMovement;
        intereactText.SetActive(false);
        player = inventory.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerInventory)
        {
            inField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController IsPlayer = other.GetComponent<PlayerController>();

        if (playerInventory)
        {
            inField = false;
            intereactText.SetActive(false);
        }
    }

    private void Update()
    {
        if (inField)
        {
            switch (currentState)
            {
                case FieldStates.Plough:
                    PloughField();
                    break;
                case FieldStates.Plant:
                    PlantCrop();
                    break;
                case FieldStates.Water:
                    PlantCrop();
                    WaterCrops();
                    break;
                case FieldStates.Growing:
                    break;
                case FieldStates.Grown:
                    PickupCrop();
                    WaterCrops();
                    PlantCrop();
                    break;
            }
        }
    }

    private void PloughField()
    {
        PloughPickup plough = playerInventory.GetCurrentItem() as PloughPickup;

        if(plough != null)
        {
            intereactText.SetActive(true);
            intereactText.GetComponent<TextMesh>().text = "Press E to Plough";
        }

        if (Input.GetButtonUp("Interact"))
        {
            if(plough != null)
            { 
                if (!ploughing)
                {
                    playerInventory.SetAnimation("IsPloughing", true);
                    coroutine = StartCoroutine(StartPloughing());
                    ploughing = true;
                    movement.DisableMovement();
                    playerInventory.DisableInput();
                }
                else
                {
                    playerInventory.SetAnimation("IsPloughing", false);
                    StopCoroutine(coroutine);
                    ploughing = false;
                    movement.EnableMovement();
                    playerInventory.EnableInput();
                }
            }
        }

        if (ploughing)
        {
            ploughTime += Time.deltaTime;
        }
    }

    IEnumerator StartPloughing()
    {
        if (ploughTime < 0)
        {
            ploughTime = Time.deltaTime;
        }

        yield return new WaitForSeconds(6 - ploughTime);
        playerInventory.SetAnimation("IsPloughing", false);
        currentState = FieldStates.Plant;
        SetPloughedField();
        movement.EnableMovement();
        playerInventory.EnableInput();
    }

    private void PlantCrop()
    {
        SeedPickup seedPickup = playerInventory.GetCurrentItem() as SeedPickup;

        if(seedPickup != null)
        {
            intereactText.SetActive(true);
            intereactText.GetComponent<TextMesh>().text = "Press E to Plant seed";
        }

        if (Input.GetButtonUp("Interact"))
        { 
            if (seedPickup != null)
            {
                movement.DisableMovement();
                playerInventory.DisableInput();
                GetPlantPos();
            }
        }

        if(currentPos != null && !planting)
        {
            player.transform.LookAt(currentPos.transform);
            player.transform.position = Vector3.MoveTowards(player.transform.position, currentPos.transform.position, Time.deltaTime * 5f);
            movement.PlayMoveAnimation();

            if (player.transform.position == currentPos.transform.position)
            {
                StartCoroutine(StartPlanting());
                planting = true;
            }
        }

    }

    IEnumerator StartPlanting()
    {
        movement.StopMoveAnimation();
        playerInventory.SetAnimation("Planting", true);
        playerInventory.PutItemOnBack();
        yield return new WaitForSeconds(5.8f);
        playerInventory.SetAnimation("Planting", false);
        PlantCurrentCrop(playerInventory.GetCurrentItem() as SeedPickup);
        playerInventory.PutItemOnFront();
        movement.EnableMovement();
        playerInventory.EnableInput();
        planting = false;
        currentPos = null;
        currentState = FieldStates.Water;
    }

    private void GetPlantPos()
    {
        foreach (GameObject pos in cropPos)
        {
            if (pos.transform.childCount == 0)
            {
                currentPos = pos;
                break;
            }
            else
            {
                currentPos = null;
            }
        }
    }

    private void PlantCurrentCrop(SeedPickup seedPickup)
    {
        GetCropObject(seedPickup);
        GameObject placeCrop = Instantiate(currentCrop, currentPos.transform);
        placeCrop.GetComponent<BasePickup>().Init(playerInventory);
        placedCrops.Add(placeCrop);
        seedPickup.UpdateSeedAmount();
    }

    public void WaterCrops()
    {
        BucketPickup bucketPickup = playerInventory.GetCurrentItem() as BucketPickup;

        if(bucketPickup != null)
        {
            intereactText.SetActive(true);
            intereactText.GetComponent<TextMesh>().text = "Press E to water crops";
        }

        if (Input.GetButtonUp("Interact"))
        {
            if(bucketPickup != null)
            {
                foreach(GameObject crop in placedCrops)
                {
                    if(crop.GetComponent<CropPickup>().NeedsWater())
                    {
                        cropToWater = crop;
                        movement.DisableMovement();
                        playerInventory.DisableInput();
                        Debug.Log("BANG!");
                        break;
                    }
                }
            }
        }

        if(cropToWater != null && !watering)
        {
            player.transform.LookAt(cropToWater.transform.parent);
            player.transform.position = Vector3.MoveTowards(player.transform.position, cropToWater.transform.position, Time.deltaTime * 5f);
            movement.PlayMoveAnimation();
            if(player.transform.position == cropToWater.transform.position)
            {
                watering = true;
                movement.StopMoveAnimation();
                cropToWater.GetComponent<CropPickup>().CropWatered();
                cropToWater = null;
                StartCoroutine(Watering());
            }
        }
    }

    IEnumerator Watering()
    {
        playerInventory.SetAnimation("Watering", true);
        yield return new WaitForSeconds(4);
        playerInventory.SetAnimation("Watering", false);
        movement.EnableMovement();
        playerInventory.EnableInput();
        watering = false;
    }

    private void PickupCrop()
    {
        ShovelPickup shovel = playerInventory.GetCurrentItem() as ShovelPickup;

        if (shovel != null)
        {
            intereactText.SetActive(true);
            intereactText.GetComponent<TextMesh>().text = "Press E to harvest crops";
        }

        if (Input.GetButtonUp("Interact"))
        {
           
            if (shovel != null)
            {
                foreach (GameObject crop in placedCrops)
                {
                    if (crop.GetComponent<CropPickup>().GetGrown())
                    {
                        cropToPick = crop;
                        movement.DisableMovement();
                        playerInventory.DisableInput();
                        break;
                    }
                }
            }
        }

        if(cropToPick != null && !picking)
        {
            player.transform.LookAt(cropToPick.transform);
            player.transform.position = Vector3.MoveTowards(player.transform.position, cropToPick.transform.position, Time.deltaTime * 5f);
            movement.PlayMoveAnimation();
            if(player.transform.position == cropToPick.transform.position)
            {
                movement.StopMoveAnimation();
                picking = true;
                StartCoroutine(CropPickup());
            }
        }
    }

    IEnumerator CropPickup()
    {
        playerInventory.SetAnimation("Digging", true);
        yield return new WaitForSeconds(5);
        playerInventory.SetAnimation("Digging", false);
        movement.EnableMovement();
        playerInventory.EnableInput();
        playerInventory.GetCurrentItem().DropItem();
        cropToPick.GetComponent<BasePickup>().SetCanPickup(true);
        playerInventory.ChangeItem(cropToPick, cropToPick.GetComponent<BasePickup>() as IPickup);
        placedCrops.Remove(cropToPick);
        cropToPick = null;
        planting = false;

    }

    private void CheckFieldCondition()
    {
        if (placedCrops.Count == 0)
        {
            ResetField();
        }
        else
        {
            currentState = FieldStates.Water;
        }
    }

    private void RefreshCropArray()
    {
        placedCrops.Clear();

        foreach (GameObject crop in removeCrops)
        {
            placedCrops.Add(crop);
        }

        removeCrops.Clear();
    }

    public void Grow()
    {
        foreach (GameObject crop in placedCrops)
        {
            CropPickup currentCrop = crop.GetComponent<CropPickup>();
            if(!currentCrop.NeedsWater())
            {
                currentCrop.Grow();
            }
            if (currentCrop.GetGrown())
            {
                currentState = FieldStates.Grown;
            }
        }

        if (currentState != FieldStates.Grown)
        {
            currentState = FieldStates.Water;
        }
    }

    private void SetPloughedField()
    {
        this.GetComponent<MeshFilter>().mesh = meshes[1];
    }

    void SetCrop(string crop)
    {

    }

    void GetCropObject(SeedPickup seed)
    {
        string crop = seed.GetCropType();
        Debug.Log(crop);

        if (crop == "Melon")
        {
            currentCrop = cropList[0];
        }
        else if (crop == "Pumpkin")
        {
            currentCrop = cropList[1];
        }
        else if (crop == "Squash")
        {
            currentCrop = cropList[2];
        }
    }

    void ResetField()
    {
        currentState = FieldStates.Plough;
        conPloughTime = 0;
        currentPloughTime = 0;
        conWaterTime = 0;
        currentWaterTime = 0;

        this.GetComponent<MeshFilter>().mesh = meshes[0];
    }

    public GameObject GetCrops()
    {
        if(placedCrops.Capacity > 0)
        {
            return placedCrops[Random.Range(0, placedCrops.Capacity - 1)];
        }
        else
        {
            return null;
        }
    }
}
