using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    private bool onHolder = false;

    GameObject turretGun;
    public GameObject mutant;

    // Start is called before the first frame update
    void Start()
    {
        turretGun = GameObject.Find("Stand");
    }

    // Update is called once per frame
    void Update()
    {
        if(onHolder)
        {
            this.transform.LookAt(mutant.transform);
        }
        else
        {
            
        }
    }

    public void OnHolder(bool on)
    {
        onHolder = on;
    }

    public bool CheckOnHolder()
    {
        return onHolder;
    }
}
