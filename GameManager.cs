using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public BuyingShop shop;
    public Shop sellingShop;
    public CropManager cropManager;
    Inventory playerInventory;
    PlayerMovement playerMovement;
    TutorialManager tutorialManager;
    Health playerHealth;
    UIManager uiManager;
    public Sprite startImage;
    [TextArea(5, 10)]
    public string startMessage;


    private void Awake()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        playerInventory = player.GetComponent<Inventory>();
        playerInventory.Init();
        playerMovement = player.GetComponent<PlayerMovement>();
        playerHealth = player.GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();

        foreach (BasePickup pickup in FindObjectsOfType<BasePickup>())
        {
            pickup.Init(playerInventory);
        }


        sellingShop.Init(playerInventory);

        foreach (SimpleField field in FindObjectsOfType<SimpleField>())
        {
            field.Init(playerInventory, cropManager, playerMovement);
        }

        if (FindObjectOfType<TutorialManager>() != null)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
            tutorialManager.Init(playerMovement, playerInventory, player);
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        shop.Init(playerInventory, playerMovement);

        uiManager.ShowTutorialMessage(startImage, startMessage);
    }

    private void Update()
    {
        if(playerHealth.IsDead())
        {
            SceneManager.LoadScene("DeathScene");
        }

        Debug.Log(Time.timeScale);
    }

}
