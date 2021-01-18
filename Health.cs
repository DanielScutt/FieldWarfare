using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float healthAmount = 100f;
    bool isDead;

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        if (healthAmount <= 0)
        {
            isDead = true;
        }
    }

    public bool IsDead()
    {
        return isDead;
    }

    public float GetHealth()
    {
        return healthAmount;
    }

    public void SetHealth(float amount)
    {
        healthAmount = amount;
    }

    public float GetMaxHealth()
    {
        return 100;
    }
}
