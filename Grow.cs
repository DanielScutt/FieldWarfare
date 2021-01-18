using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grow : MonoBehaviour
{
    [SerializeField] int daysToGrow;
    [SerializeField] Mesh[] growth;

    int currentDay = 0;
    int stageOne;
    int stageTwo;
    int stageThree;
    bool grown = false;
    PickUp pickup;
    MeshCollider meshColl;

    private void Awake()
    {
        GetComponentInChildren<MeshFilter>().mesh = growth[currentDay];
        pickup = this.gameObject.GetComponent<PickUp>();
        meshColl = GetComponentInChildren<MeshCollider>();

    }

    private void Start()
    {
        stageOne = daysToGrow / 3;
        stageTwo = (daysToGrow / 3) * 2;
        stageThree = (daysToGrow / 3) * 3;
    }

    private void Update()
    {
        if (Input.GetButtonDown("CycleDay"))
        {
            DayOver();
        }
    }

    void DayOver()
    {
        currentDay++;

        if(currentDay <= stageOne)
        {
            GetComponentInChildren<MeshFilter>().mesh = growth[0];
            meshColl.sharedMesh = growth[0];
        }
        else if(currentDay <= stageTwo)
        {
            GetComponentInChildren<MeshFilter>().mesh = growth[1];
            meshColl.sharedMesh = growth[1];
        }
        else if (currentDay < stageThree)
        {
            GetComponentInChildren<MeshFilter>().mesh = growth[2];
            meshColl.sharedMesh = growth[2];
        }
        else if(currentDay == daysToGrow)
        {
            GetComponentInChildren<MeshFilter>().mesh = growth[3];
            meshColl.sharedMesh = growth[3];
        }

        Debug.Log(currentDay);

        

        if (currentDay >= daysToGrow)
        {
            pickup.SetCanPickUp(true);
            grown = true;
        }
        else
        {
            pickup.SetCanPickUp(false);
        }
    }
}
