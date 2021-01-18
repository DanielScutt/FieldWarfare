using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Price : MonoBehaviour
{
    [SerializeField] float price;

    public float GetPrice()
    {
        return price;
    }
}
