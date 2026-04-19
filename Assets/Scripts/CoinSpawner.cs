using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform[] spawnPoints;
    public float respawnDelay = 2f;

    private GameObject currentCoin;
    // Das Gedächtnis des Spawners
    private int lastSpawnIndex = -1; 

    void Start()
    {
        SpawnCoin();
    }

    public void OnCoinCollected()
    {
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(respawnDelay);
        SpawnCoin();
    }

    void SpawnCoin()
    {
       
        if (spawnPoints == null || spawnPoints.Length == 0) return;

        int randomIndex;

       
        if (spawnPoints.Length > 1)
        {
            do
            {
                randomIndex = Random.Range(0, spawnPoints.Length);
            } while (randomIndex == lastSpawnIndex); 
        }
        else
        {
            randomIndex = 0; 
        }

        
        lastSpawnIndex = randomIndex;
        Transform spawn = spawnPoints[randomIndex];

        currentCoin = Instantiate(coinPrefab, spawn.position, Quaternion.identity);

        Coin coinScript = currentCoin.GetComponent<Coin>();
        if (coinScript != null)
        {
            coinScript.spawner = this;
            coinScript.regularSpawn = true;
        }
        
        Debug.Log("Spawned Coin at Point: " + randomIndex);
    }
}