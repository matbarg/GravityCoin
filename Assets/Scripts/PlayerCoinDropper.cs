using UnityEngine;

public class PlayerCoinDropper : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnRadius = 0.5f;

    public void DropCoins(int amount, Vector2 origin)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector2 spawnPos = origin + Random.insideUnitCircle * spawnRadius;

            Instantiate(coinPrefab, spawnPos, Quaternion.identity);
        }
    }
}
