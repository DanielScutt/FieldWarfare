using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 7.5f;
    public float timeToZeroToMax = 0.1f;
    [SerializeField] float rotateSpeed;
    [SerializeField] float cameraSpeed;
    [SerializeField] GameObject mesh;
    [SerializeField] Transform cam;
    [SerializeField] GameObject camVert;
    [SerializeField] Transform itemPos;
    public Animator animator;

    bool onMainCamera;

    public Transform mainCam;
    public Transform aimingCam;
    public GameObject mainCamVert;
    public GameObject aimingCamVert;
    public Transform body;

    public Transform[] itemPositions;

    public bool hasItem = false;
    public IPickup currentItem;
    GameObject itemObject;
    public Transform backPos;


    float minRotation = -45;
    float maxRotation = 45;
    float zPos;
    float xPos;
    float lookY;
    float lookX;
    float speed;
    float currYDir;
    float prevYDir;
    float accelPerSec;
    Quaternion startRotation;
    Rigidbody rb;
    Vector3 movement;

    Vector3 currentRotation;
    Quaternion currentQuat;

    float savedBodyRotation;
    Vector3 startingPostion;
    Quaternion camRotation;

    public void Init()
    {
        //Getting components
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();

        //Set Acceleration of the player when moving.
        accelPerSec = maxSpeed / timeToZeroToMax;
        speed = 0;
        onMainCamera = true;
        ChangeCamera(onMainCamera);

        savedBodyRotation = body.transform.rotation.z;
        startingPostion = aimingCam.transform.position;
        camRotation = aimingCam.transform.rotation;
    }

    void Update()
    {


        //Checks if the player has an item, if they do they can drop the item.
        if (hasItem)
        {
            if (Input.GetButton("Drop"))
            {
                DropItem();
            }
        }

        //Checks if the fire button has been pressed (and will fire the weapon if the player has one)
        if(Input.GetAxis("Attack") == 1)
        {
            IWeapon weapon = currentItem as IWeapon;

            if(weapon != null)
            {
                weapon.Fire();
            }
        }

        //Will reload the weapon if the player has a weapon
        if (Input.GetButtonDown("Reload"))
        {
            IWeapon weapon = currentItem as IWeapon;

            if (weapon != null)
            {
                weapon.Reload();
            }
        }

        //Pressing the sprint button increases the maxSpeed var and updates the acceleration of the player.
        if(Input.GetButtonDown("Sprint"))
        {
            maxSpeed *= 1.5f;
            accelPerSec = maxSpeed / timeToZeroToMax;
        }

        //let going of the sprint button resets the speed and acceleration
        if (Input.GetButtonUp("Sprint"))
        {
            maxSpeed /= 1.5f;
            accelPerSec = maxSpeed / timeToZeroToMax;
        }

        if(Input.GetMouseButtonDown(1))
        {
            onMainCamera = false;
            ChangeCamera(onMainCamera);
        }

        if(Input.GetMouseButtonUp(1))
        {
            onMainCamera = true;
            ChangeCamera(onMainCamera);
        }

        //Sets the Input Axis
        zPos = Input.GetAxis("Vertical");

        if(onMainCamera)
        {
            savedBodyRotation = body.transform.rotation.z;
            MainControls();
        }
        else
        {
            AimingControls();
        }

    }

    private void AimingControls()
    {
        //Move the player in the direction of the camera
        Vector3 moveForward = mesh.transform.forward * zPos;

        //Combines the Vectors together to get the direction the player is moving.
        movement = moveForward;

        animator.SetFloat("moving", zPos);
    }

    private void MainControls()
    {
        Vector3 forward = new Vector3(mainCam.transform.forward.x, 0, mainCam.transform.forward.z);

        //Move the player in the direction of the camera
        Vector3 moveForward = forward * zPos;

        //Combines the Vectors together to get the direction the player is moving.
        movement = moveForward;

        animator.SetFloat("moving", zPos);

    }

    private void FixedUpdate()
    { 

        //Moves the character.
        if(movement.magnitude > 0.4f)
        {
                MoveCharacter(movement);
        }
    }

    void ChangeCamera(bool onMainCamera)
    {
        mainCamVert.SetActive(onMainCamera);
        aimingCamVert.SetActive(!onMainCamera);

        if(onMainCamera)
        {
            mainCamVert.transform.rotation = mesh.transform.rotation;
        }
    }

    private void LateUpdate()
    {
        if(!onMainCamera)
        {
            savedBodyRotation += lookY;

            savedBodyRotation = Mathf.Clamp(savedBodyRotation, -80, 80);

            Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, savedBodyRotation));

            body.transform.Rotate(rotation.eulerAngles);

            
        }

        Quaternion cameraRotation = Quaternion.Euler(mesh.transform.localEulerAngles.x, mainCam.transform.localEulerAngles.y, mesh.transform.localEulerAngles.z);

        mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, cameraRotation, Time.deltaTime * rotateSpeed);
    }

    public float GetMoney()
    {
        return this.GetComponent<Wallet>().GetBalance();
    }

    private void MoveCharacter(Vector3 movement)
    {
        //Causes the player speed to slowly increase to max speed.
        ApplySpeed();
        //Moves the player to the new vector.
        rb.MovePosition(this.transform.localPosition + (movement * speed * Time.deltaTime));

        
    }

    private void ApplySpeed()
    {
        //Applys the acceleration to the speed.
        speed += (accelPerSec * Time.deltaTime);
        //Clamps the speed between the lowest or highest possible number.
        speed = Mathf.Min(speed, maxSpeed);
    }

    public void DropItem()
    {
        currentItem.DropItem();
        SetHasItem(false, null);
        animator.SetBool("CarryingTool", false);
        animator.SetBool("CarryingSeeds", false);
        animator.SetBool("CarryingGun", false);
        animator.SetBool("IsPloughing", false);
    }

    public bool GetHasItem()
    {
        return hasItem;
    }

    public IPickup GetCurrentItem()
    {
        return currentItem;
    }

    public void SetHasItem(bool gotItem, IPickup item)
    {
        currentItem = item;
        hasItem = gotItem;
    }

    private void OnTriggerStay(Collider other)
    {
        if (hasItem)
        {
            return;
        }
        else
        {
            if (Input.GetButton("Interact"))
            {
                //Gets BasePickup Component
                BasePickup pickup = other.GetComponent<BasePickup>();
                //Gets the interface IPickup
                IPickup item = pickup as IPickup;

                if (item != null)
                {
                    if(item.GetCanPickup())
                    {
                        //Calls function with ref of the object and playerPos
                        item.PickupItem(pickup.gameObject, itemPositions);
                        //Tells the function that the player has an item, making a ref of the item interface
                        SetHasItem(true, item);
                        itemObject = pickup.gameObject;
                    }
                }
            }
        }
    }

    public void IsPloughing(bool isPloughing)
    {
        bool ploughing = isPloughing;
        Debug.Log(ploughing);
        animator.SetBool("IsPloughing", ploughing);
    }
}
