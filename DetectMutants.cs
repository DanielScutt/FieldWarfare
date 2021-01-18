using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectMutants : MonoBehaviour
{
    public List<GameObject> mutant;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<MutantController>())
        {
            mutant.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<MutantController>())
        {
            mutant.Remove(other.gameObject);
        }
    }

    public void MutantDied(GameObject deadMutant)
    {
        mutant.Remove(deadMutant);
    }

    private void Update()
    {
        if (mutant.Count == 0)
        {
           
        }
        else
        {
            if (mutant[0] == null)
            {
                mutant.Remove(mutant[0]);
            }
        }

    }

    public GameObject GetMutant()
    {
        if(mutant.Count == 0)
        {
            return null;
        }

        return mutant[0];
    }
}
