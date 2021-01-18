using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantSpawner : MonoBehaviour
{
    [SerializeField] float spawnTime;
    [SerializeField] float minTime, maxTime;
    public GameObject[] mutants;

    MutantManager mutantManager;
    bool spawning = false;
    GameTime gameTime;

    private void Start()
    {
        mutantManager = FindObjectOfType<MutantManager>().GetComponent<MutantManager>();
        gameTime = FindObjectOfType<GameTime>().GetComponent<GameTime>();
    }

    private void Update()
    {
        if(gameTime.IsNightTime())
        {
            if(!spawning)
            {
                StartSpawning();
                spawning = true;
            }
        }
        else
        {
            if(spawning)
            {
                StopCoroutine(SpawnMutant());
                spawning = false;
            }

            if(this.transform.childCount > 0)
            {
                int children = this.transform.childCount;

                for(int i = children -1; i>= 0; i--)
                {
                    DestroyImmediate(this.transform.GetChild(i).gameObject);
                }
            }
        }
    }

    IEnumerator SpawnMutant()
    {
        mutantManager.StartedSpawning();
        yield return new WaitForSeconds(spawnTime);
        int randomIndex = Random.Range(0, mutants.Length);
        GameObject randomMutant = mutants[randomIndex];
        GameObject spawnedMutant = Instantiate(randomMutant, this.transform) as GameObject;
        mutantManager.AddMutant(spawnedMutant);
        if(!mutantManager.WaveOver())
        {
            StartSpawning();
        }
    }

    void StartSpawning()
    {
        spawnTime = Random.Range(minTime, maxTime);
        StartCoroutine(SpawnMutant());
    }
}
