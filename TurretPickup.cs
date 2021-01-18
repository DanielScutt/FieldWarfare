using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPickup : BasePickup, ITurret
{
    enum State { TurnOff, TurnOn, NoAmmo, NoTargets};         

    GameObject mutant;
    MutantManager mutantManager;
    public DetectMutants coll;

    public float damage = 10f;
    public int ammo = 100;
    public float fireRate = 1f;
    public float range = 10f;
    public ParticleSystem shotFX;
    public int damagelevel;
    public int fireRateLevel;

    public bool turnedOn;
    float distance;
    bool firing;
    float mutantDistance;
    TurretHolder currentHolder;
    [SerializeField] State currentState;
    public AudioSource shotSound;

    GameObject mutantClose;

    public override void Init(Inventory inventory)
    {
        base.Init(inventory);
        mutantManager = FindObjectOfType<MutantManager>().GetComponent<MutantManager>();
        currentState = State.TurnOff;
    }

    public void UpgradeDamage(float newDamage)
    {
        damage = newDamage;
        damagelevel++;
    }

    public void UpgradeFireRate(float newFireRate)
    {
        fireRate = newFireRate;
        fireRateLevel++;
    }

    public bool IsMaxDamageLevel()
    {
        return damagelevel == 10;
    }


    public bool IsMaxFireRateLevel()
    {
        return fireRateLevel == 10;
    }

    public override void SetItemInfo()
    {
        itemInfo.itemName = "Turret";
        itemInfo.itemAmount = "null";
        itemInfo.itemControls = "Press Q to Drop/Equip to Turret Base";
    }


    public void PutOnHolder(GameObject holder, Transform position)
    {
        this.transform.parent = position;
        this.transform.position = position.position;
        this.transform.rotation = position.rotation;
        rb.isKinematic = true;
        currentHolder = holder.GetComponent<TurretHolder>();
        TurnOn();
    }

    public override void PickupItem(GameObject item, Transform[] itemPositions)
    {
        base.PickupItem(item, itemPositions);

        this.transform.position = itemPositions[4].transform.position;
        this.transform.rotation = itemPositions[4].transform.rotation;
        this.transform.parent = itemPositions[4];
        playerInventory.SetAnimation("CarryingSeeds", true);
    }

    public override void DropItem()
    {
        base.DropItem();
        playerInventory.SetAnimation("CarryingSeeds", false);
    }

    IEnumerator FireRate()
    {
        while(currentState == State.TurnOn)
        {
            yield return new WaitForSeconds(fireRate);
            Fire();
        }
    }

    public virtual void TurnOn()
    {
        if(currentState != State.TurnOn)
        {
            currentState = State.TurnOn;
            if (currentHolder.HasAmmo())
            {
                StartCoroutine(FireRate());
            }
            else
            {
                currentState = State.NoAmmo;
            }
        }
    }

    public virtual void Fire()
    {
        if(currentState == State.TurnOn)
        {
            if (mutantClose != null)
            {
                shotSound.Play();
                mutantClose.GetComponent<MutantController>().TakeDamage(damage);
                currentHolder.Fired();
                shotFX.Play();
                if (mutantClose.GetComponent<MutantController>().IsDead())
                {
                    coll.MutantDied(mutantClose);
                }
            }
        }
        else
        {
            StopAllCoroutines();
        }
    }

    public virtual void TurnOff()
    {
        StopAllCoroutines();
        currentState = State.TurnOff;
    }

    private void CheckForMutants()
    {
        mutantClose = coll.GetMutant();

        if (mutantClose != null)
        {
            transform.LookAt(mutantClose.transform);
            TurnOn();
        }
    }

    private void StandBy()
    {
        transform.LookAt(null);

        if(currentHolder != null)
        {
            if (currentHolder.HasAmmo())
            {
                TurnOn();
            }
        }

    }

    private void Update()
    {
        switch(currentState)
        {
            case State.TurnOff:
                StandBy();
            break;
            case State.TurnOn:
                CheckForMutants();
                break;
            case State.NoAmmo:
                StandBy();
                break;
            case State.NoTargets:
                CheckForMutants();
                break;
        }
    }
}
