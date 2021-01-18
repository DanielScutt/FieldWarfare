using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    float balance = 2000;

    public void AddToBalance(float income)
    {
        balance += income;
        Debug.Log(balance.ToString());
    }

    public void MinusBalance(float spent)
    {
        balance -= spent;
        Debug.Log(balance.ToString());
    }

    public float GetBalance()
    {
        return balance;
    }
}
