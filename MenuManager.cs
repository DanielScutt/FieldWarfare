using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    InputMaster input;
    public GameObject pauseScreen;
    bool paused = false;

    private void Start()
    {
        input = new InputMaster();
        input.Enable();
        input.Player.Pause.performed += Pause;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        if(!paused)
        {
            pauseScreen.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            pauseScreen.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
            paused = false;
        }
    }

    public void PlayAgain()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene("MainLevel");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void OnDisable()
    {
        input.Disable();
    }
}
