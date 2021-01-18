using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public Transform cam;
    public float maxSpeed;
    public float rotateSpeed;
    public float accTime;
    public CinemachineBrain camBrain;

    InputMaster input;
    Rigidbody rb;
    Animator animator;

    float speed;
    float t;
    float moveForward;
    float moveRight;
    float runSpeed;
    float walkSpeed;

    private void Awake()
    {
        InitVars();
        InitEvents();
    }

    private void InitVars()
    {
        input = new InputMaster();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        walkSpeed = maxSpeed;
        runSpeed = walkSpeed * 1.5f;
    }

    private void InitEvents()
    {
        input.Player.Movement.performed += cxt => Move(cxt.ReadValue<Vector2>());
        input.Player.Movement.canceled += cxt => Move(cxt.ReadValue<Vector2>());
        input.Player.Sprint.performed += Sprint;
        input.Player.Sprint.canceled += Sprint;
    }

    private void Sprint(InputAction.CallbackContext cxt)
    {
        if(cxt.ReadValue<float>() > 0)
        {
            maxSpeed = runSpeed;
        }
        else
        {
            maxSpeed = walkSpeed;
        }
    }

    private void Move(Vector2 dir)
    {
        moveForward = dir.y;
        animator.SetFloat("moving", moveForward);
    }

    private void Update()
    {
        //Check if the player has moved forward.
        if (Mathf.Abs(moveForward) > 0)
        {
            RotatePlayer();
        }

        //Check if the player has moved
        if (Mathf.Abs(moveForward) > 0 || Mathf.Abs(moveRight) > 0)
        {
            SetSpeed();
        }
        else
        {
            ResetSpeed();
        }

        MovePlayer();

    }

    private void MovePlayer()
    {
        Vector3 forward = cam.transform.forward;
        forward.y = 0;
        forward = forward.normalized;

        Vector3 right = this.transform.right;
        right.y = 0;
        right = right.normalized;

        transform.position += (forward * moveForward + right * moveRight) * Time.deltaTime * speed;
    }

    private void ResetSpeed()
    {
        t = 0;
        speed = 0;
    }

    private void SetSpeed()
    {
        t += Time.deltaTime / accTime;
        speed = Mathf.Lerp(0, maxSpeed, t);
    }

    private void RotatePlayer()
    {
        Quaternion newRotation = Quaternion.Euler(0, cam.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, Time.deltaTime * rotateSpeed);
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    public bool IsMoving()
    {
        return moveForward == 1;
    }

    public void DisableMovement()
    {
        input.Disable();
    }

    public void EnableMovement()
    {
        input.Enable();
    }

    public void PlayMoveAnimation()
    {
        animator.SetFloat("moving", 1f);
    }


    public void StopMoveAnimation()
    {
        animator.SetFloat("moving", 0f);
    }

    public void DisableCameraControls()
    {
        camBrain.enabled = false;
    }

    public void EnableCameraControls()
    {
        camBrain.enabled = true;
    }
}
