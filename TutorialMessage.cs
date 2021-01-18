using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMessage : MonoBehaviour
{
    public Image tutorialPic;
    public Text tutorialDes;
    public GameObject tutorialWindow;

    public void Init(Sprite tutorialImage, string tutorialMessage)
    {
        tutorialWindow.SetActive(true);
        tutorialPic.sprite = tutorialImage;
        tutorialDes.text = tutorialMessage;

        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void CloseMessage()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
        tutorialWindow.SetActive(false);
    }
}
