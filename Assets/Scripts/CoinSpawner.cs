using UnityEngine;
using System.Collections;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform[] spawnPoints;
    public float respawnDelay = 2f;

    private GameObject currentCoin;

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
        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        currentCoin = Instantiate(coinPrefab, spawn.position, Quaternion.identity);

        Coin coinScript = currentCoin.GetComponent<Coin>();
        coinScript.spawner = this;
		coinScript.regularSpawn = true;
        
        Debug.Log("Spawned Coin");
    }
}