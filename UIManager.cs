using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    CropManager cropManager;
    public GameTime gameTime;
    Wallet playerWallet;
    public TutorialMessage tutorialWindow;

    // Start is called before the first frame update
    void Start()
    {
        cropManager = FindObjectOfType<CropManager>().GetComponent<CropManager>();
        gameTime = FindObjectOfType<GameTime>().GetComponent<GameTime>();
        playerWallet = FindObjectOfType<Inventory>().GetComponent<Wallet>();
    }

    public bool IsNightTime()
    {
        return gameTime.IsNightTime();
    }

    public float GetCropCount()
    {
        return cropManager.GetCropCount();
    }

    public IPickup item()
    {
        return null;//playerController.GetCurrentItem();
    }

    public float GetMoney()
    {
        return playerWallet.GetBalance();
    }

    public void ShowTutorialMessage(Sprite tutorialImage, string tutorialMessage)
    {
        tutorialWindow.Init(tutorialImage, tutorialMessage);
    }
}
