using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantManager : MonoBehaviour
{
    public List<Transform> mutantList;
    public int mutantCount = 0;
    public int waveCount = 10;

    int spawnCount;
    float closestDistance = 0;
    bool firstTime = true;
    Transform closetMutant;

    GameTime gameTime;
    bool spawned = false;

    private void Start()
    {
        gameTime = FindObjectOfType<GameTime>().GetComponent<GameTime>();
    }

    private void Update()
    {
        if(MutantDead() && gameTime.IsNightTime() && spawned)
        {
            spawned = false;
            waveCount += Random.Range(1, 5);
            spawnCount = 0;
            mutantCount = 0;
            gameTime.SetDaytime();
            gameTime.NextDay();
        }
    }

    public void AddMutant(GameObject newMutant)
    {
        mutantCount++;
        Transform mutantTransform = newMutant.GetComponent<Transform>();
        mutantList.Add(mutantTransform);
        spawned = true;
    }

    private void KillAll()
    {
        mutantList.Clear();
    }

    public void RemoveMutant(Transform removeMutant)
    {
        mutantList.Remove(removeMutant);
    }

    public float GetClosestDistance()
    {
        return closestDistance;
    }

    public bool WaveOver()
    {
        return spawnCount >= waveCount;
    }

    public bool MutantDead()
    {
        return mutantList.Count == 0;
    }

    public void StartedSpawning()
    {
        spawnCount++;
    }
}
