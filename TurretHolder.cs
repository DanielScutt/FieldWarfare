using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretHolder : MonoBehaviour
{
    enum HolderStates { NoTurret, HasTurret, NeedsAmmo, NeedsRepair }

    [SerializeField] GameObject turretPosition;

    public int ammo;
    public TextMesh ammoText;
    public TextMesh damageLevelText;
    public TextMesh fireRateLevelText;
    public int maxAmmo = 100;
    public Sprite tutorialPic;
    [TextArea(5, 10)]
    public string tutorialDes;
    public AudioSource placedTurretSound;


    [SerializeField] HolderStates currentState;
    private TurretPickup currentTurret;
    private GameObject player;
    private Inventory playerInventory;
    private bool inTrigger;
    private bool tutorial = false;
    

    private void Start()
    {
        ammoText.text = ammo.ToString();
        currentState = HolderStates.NoTurret;
        player = FindObjectOfType<PlayerMovement>().gameObject;
        playerInventory = player.GetComponent<Inventory>();
        fireRateLevelText.gameObject.SetActive(false);
        damageLevelText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            inTrigger = true;

            if (playerInventory.GetTurretTut() == false)
            {
                FindObjectOfType<UIManager>().GetComponent<UIManager>().ShowTutorialMessage(tutorialPic, tutorialDes);
                playerInventory.SetTurretTut();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            inTrigger = false;
        }
    }

    private void Update()
    {
        if(inTrigger)
        {

            switch(currentState)
            {
                case HolderStates.NoTurret:
                    HasNoTurret();
                break;
                case HolderStates.HasTurret:
                    HasTurret();
                    DamageUpgrades();
                    FireRateUpgrades();
                    break;
            };

            AddingAmmo();
        }
    }

    void DamageUpgrades()
    {
            if (Input.GetButtonUp("Interact"))
            {
            if (!currentTurret.IsMaxDamageLevel())
            {
                DamageUpgrade damageUpgrade = playerInventory.GetCurrentItem() as DamageUpgrade;

                if (damageUpgrade != null)
                {
                    currentTurret.UpgradeDamage(damageUpgrade.UpgradeStat(currentTurret.damage));
                    currentTurret.TurnOn();
                    damageLevelText.text = currentTurret.damagelevel.ToString();
                }
            }
        }
    }

    void FireRateUpgrades()
    {

            if (Input.GetButtonUp("Interact"))
            {
            if (!currentTurret.IsMaxFireRateLevel())
            {
                FireRateUpgrade fireRateUpgrade = playerInventory.GetCurrentItem() as FireRateUpgrade;

                if (fireRateUpgrade != null)
                {
                    currentTurret.UpgradeFireRate(fireRateUpgrade.UpgradeStat(currentTurret.fireRate));
                    currentTurret.TurnOn();
                    fireRateLevelText.text = currentTurret.fireRateLevel.ToString();
                }
            }
        }
    }

    private void AddingAmmo()
    {
            AmmoPickup ammoPickup = playerInventory.GetCurrentItem() as AmmoPickup;
        if (Input.GetButtonUp("Interact"))
        {
            if (ammoPickup != null)
            {
                ammo += ammoPickup.GetAmmo();
                if (ammo > maxAmmo)
                {
                    ammoPickup.UsedAmmo(ammo - maxAmmo);
                    ammo = maxAmmo;
                }
                else
                {
                    ammoPickup.UsedAmmo(0);
                }
            }
        }
        ammoText.text = ammo.ToString();
    }

    private void HasNoTurret()
    {
            if (Input.GetButtonDown("Interact"))
            {
            TurretPickup turret = playerInventory.GetCurrentItem() as TurretPickup;
            if (turret != null)
            {
                placedTurretSound.Play();
                currentState = HolderStates.HasTurret;
                currentTurret = turret;
                playerInventory.DropItem();
                currentTurret.PutOnHolder(this.gameObject, turretPosition.transform);
                currentTurret.TurnOn();
                fireRateLevelText.gameObject.SetActive(true);
                damageLevelText.gameObject.SetActive(true);
                fireRateLevelText.text = currentTurret.fireRateLevel.ToString();
                damageLevelText.text = currentTurret.damagelevel.ToString();
            }
        }
    }

    private void HasTurret()
    {
            if (turretPosition.transform.childCount == 0)
            {
                currentState = HolderStates.NoTurret;
                currentTurret.TurnOff();
                currentTurret = null;
            fireRateLevelText.gameObject.SetActive(false);
            damageLevelText.gameObject.SetActive(false);
        }
    }

    public void Fired()
    {
        ammo--;
        ammoText.text = ammo.ToString();
        if(ammo <= 0)
        {
            currentTurret.TurnOff();
        }
    }

    public bool HasAmmo()
    {
        return ammo > 0;
    }
}
