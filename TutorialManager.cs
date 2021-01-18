using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class TutorialManager : MonoBehaviour
{
    public delegate void MoveMouse();
    public static event MoveMouse onMove;
    public CinemachineBrain cam;
    public GameObject startMessage;
    public Transform PlayerUI;
    public GameObject Challenges;

    InputMaster inputs;

    Quaternion startingRotation;

    public void Init(PlayerMovement movement, Inventory inventory, GameObject gameObject)
    {
        startMessage.SetActive(true);
        cam.enabled = false;
        inputs = new InputMaster();
        inputs.Enable();
        movement.DisableMovement();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartGame()
    {
        cam.enabled = true;
        startMessage.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
