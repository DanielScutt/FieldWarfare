using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleField : MonoBehaviour
{
    enum State { Plough, Plant, Water, Growing, Grown }

    State currentState;

    public Mesh[] fieldMeshes;
    public GameObject[] cropList;
    public List<Transform> cropPos;
    public List<GameObject> cropsInField;
    public Transform plantPosition;
    public Material material;
    public TextMesh tipText;
    public Material tipMaterial;
    public string[] textStates;

    public float ploughTime;
    public Sprite tutorialPic;
    [TextArea(5, 10)]
    public string tutorialDes;

    private CropManager cropManager;
    private Inventory playerInventory;
    private PlayerMovement playerMovement;
    private GameObject player;
    private Transform movePos;
    private GameObject currentCrop;

    private bool tutorial = false;
    private bool inField;
    private float damageFieldStat;

    public void Init(Inventory inventory, CropManager cropManagerRef, PlayerMovement movement)
    {
        Debug.Log("Test");
        damageFieldStat = GetComponent<Health>().GetHealth() - 40;
        tipText.text = textStates[0];
        tipMaterial.color = Color.red;
        material.color = Color.white;
        cropManager = cropManagerRef;
        cropManager.AddField(this.gameObject);
        playerInventory = inventory;
        currentState = State.Plough;
        playerMovement = movement;
        //InteractText
        player = inventory.gameObject;
    }

    public void Locked()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            inField = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            inField = false;
        }
    }

    public Transform GetTransform()
    {
        return plantPosition;
    }

    private void Update()
    {
        if (inField)
        {
            if(playerInventory.GetFieldTut() == false)
            {
                FindObjectOfType<UIManager>().GetComponent<UIManager>().ShowTutorialMessage(tutorialPic, tutorialDes);
                playerInventory.SetFieldTut();
            }

            switch (currentState)
            {
                case State.Plough:
                    PloughField();
                    break;
                case State.Plant:
                    PlantCrops();
                    break;
                case State.Water:
                    WaterCrops();
                    break;
                case State.Grown:
                    PickupCrops();
                    break;
            }
        }

        if (HasCrops())
        {
            FieldHealth();
        }
        else
        {

        }
    }

    private void FieldHealth()
    {
        if (GetComponent<Health>().GetHealth() < damageFieldStat)
        {
            foreach(GameObject crop in cropsInField)
            {
                cropManager.RemoveCrop(crop);
                Destroy(crop, 1f);
                cropsInField.Remove(crop);
                break;
            }

            damageFieldStat -= 40;
        }
    }

    private void PloughField()
    {
        PloughPickup plough = playerInventory.GetCurrentItem() as PloughPickup;

        if (CropsBeenPicked())
        {
            tipText.text = textStates[0];
        }


        if (Input.GetButtonUp("Interact"))
        {
            if (CropsBeenPicked())
            {
                if (plough != null)
                {
                    playerInventory.SetAnimation("IsPloughing", true);
                    StartCoroutine(Ploughing());
                    playerMovement.DisableMovement();
                    playerInventory.DisableInput();
                }
            }
        }
    }

    IEnumerator Ploughing()
    {
        yield return new WaitForSeconds(ploughTime);
        playerInventory.SetAnimation("IsPloughing", false);
        cropsInField.Clear();
        currentState = State.Plant;
        ChangeField();
        playerMovement.EnableMovement();
        playerInventory.EnableInput();
    }

    private void ChangeField()
    {
        switch (currentState)
        {
            case State.Plough:
                if (CropsBeenPicked())
                {
                    tipText.text = textStates[0];
                }
                else
                {
                    tipText.text = textStates[4];
                }
                material.SetColor("_BaseColor", Color.white);
                this.GetComponent<MeshFilter>().mesh = fieldMeshes[0];
                break;
            case State.Plant:
                tipText.text = textStates[1];
                this.GetComponent<MeshFilter>().mesh = fieldMeshes[1];
                break;
            case State.Water:
                tipText.text = textStates[2];
                material.SetColor("_BaseColor", Color.white);
                break;
            case State.Growing:
                tipText.text = "";
                material.SetColor("_BaseColor", Color.grey);
                break;
        }


    }


    private void PlantCrops()
    {
        SeedPickup seedPickup = playerInventory.GetCurrentItem() as SeedPickup;

        if (seedPickup != null)
        {
            //Text stuff
        }

        if (Input.GetButtonUp("Interact"))
        {
            if (seedPickup != null)
            {
                playerMovement.DisableMovement();
                playerInventory.DisableInput();
                movePos = plantPosition;
                player.transform.LookAt(movePos.transform);
            }
        }

        if (movePos != null)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movePos.position, Time.deltaTime * 5f);
            playerMovement.PlayMoveAnimation();

            if (player.transform.position == movePos.position)
            {
                StartCoroutine(StartPlanting());
                movePos = null;
            }
        }
    }

    IEnumerator StartPlanting()
    {
        playerMovement.StopMoveAnimation();
        playerInventory.SetAnimation("Planting", true);
        playerInventory.PutItemOnBack();
        currentState = State.Water;
        yield return new WaitForSeconds(5.8f);
        playerInventory.SetAnimation("Planting", false);
        PlantCurrentCrop(playerInventory.GetCurrentItem() as SeedPickup);
        playerInventory.PutItemOnFront();
        playerMovement.EnableMovement();
        playerInventory.EnableInput();
        ChangeField();
    }

    private void PlantCurrentCrop(SeedPickup seedPickup)
    {
        GetCropObject(seedPickup);
        foreach (Transform pos in cropPos)
        {
            GameObject crop = Instantiate(currentCrop, pos);
            crop.GetComponent<BasePickup>().Init(playerInventory);
            cropsInField.Add(crop);
            seedPickup.Used();
        }
    }

    private void WaterCrops()
    {
        BucketPickup bucket = playerInventory.GetCurrentItem() as BucketPickup;

        if (bucket != null)
        {
            //text stuff
        }

        if (Input.GetButtonUp("Interact"))
        {
            if (bucket != null)
            {
                playerMovement.DisableMovement();
                playerInventory.DisableInput();
                movePos = plantPosition;
                player.transform.LookAt(movePos);
            }
        }

        if (movePos != null)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movePos.position, Time.deltaTime * 5f);
            playerMovement.PlayMoveAnimation();
            if (player.transform.position == movePos.position)
            {
                playerMovement.StopMoveAnimation();
                WaterAllCrops();
                movePos = null;
                StartCoroutine(Watering());
            }
        }
    }

    IEnumerator Watering()
    {
        playerInventory.SetAnimation("Watering", true);
        currentState = State.Growing;
        yield return new WaitForSeconds(4);
        playerInventory.SetAnimation("Watering", false);
        playerMovement.EnableMovement();
        playerInventory.EnableInput();
        ChangeField();
    }

    private void WaterAllCrops()
    {
        foreach (GameObject crop in cropsInField)
        {
            crop.GetComponent<CropPickup>().CropWatered();
        }
    }

    public void Grow()
    {
        foreach (GameObject crop in cropsInField)
        {
            CropPickup currentCrop = crop.GetComponent<CropPickup>();
            if (!currentCrop.NeedsWater())
            {
                currentCrop.Grow();
            }
            if (currentCrop.GetGrown())
            {
                tipText.text = textStates[3];
                currentState = State.Grown;
            }
        }

        if (currentState != State.Grown)
        {
            if(!HasCrops())
            {
                    currentState = State.Plough;
                    ChangeField();
                    damageFieldStat = GetComponent<Health>().GetHealth() - 40;
                    GetComponent<Health>().SetHealth(600);
            }
            else
            {
                currentState = State.Water;
                ChangeField();
            }
        }
    }

    private void PickupCrops()
    {
        ShovelPickup shovel = playerInventory.GetCurrentItem() as ShovelPickup;

        if (shovel != null)
        {
            //text stuff
        }

        if (Input.GetButtonUp("Interact"))
        {
            if (shovel != null)
            {
                playerInventory.DisableInput();
                playerMovement.DisableMovement();
                movePos = plantPosition;
                player.transform.LookAt(plantPosition);
            }
        }

        if (movePos != null)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, movePos.position, Time.deltaTime * 5f);
            playerMovement.PlayMoveAnimation();
            if (player.transform.position == movePos.position)
            {
                playerMovement.StopMoveAnimation();
                WaterAllCrops();
                movePos = null;
                StartCoroutine(CropPickup());
            }
        }
    }

    IEnumerator CropPickup()
    {
        playerInventory.SetAnimation("Digging", true);
        yield return new WaitForSeconds(5);
        playerInventory.SetAnimation("Digging", false);
        playerMovement.EnableMovement();
        playerInventory.EnableInput();
        ChangeCrop();
        currentState = State.Plough;
        ChangeField();
    }

    private bool CropsBeenPicked()
    {
        foreach (Transform crop in cropPos)
        {
            if (crop.childCount == 1)
            {
                return false;
            }
        }

        return true;
    }

    private void ChangeCrop()
    {
        foreach (GameObject crop in cropsInField)
        {
            crop.GetComponent<BasePickup>().SetCanPickup(true);
        }
    }

    void GetCropObject(SeedPickup seed)
    {
        string crop = seed.GetCropType();

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
        else if(crop == "Beetroot")
        {
            currentCrop = cropList[3];
        }
        else if(crop == "Lettuce")
        {
            currentCrop = cropList[4];
        }
    }

    public bool HasCrops()
    {
        return cropsInField.Count > 0;
    }
}
