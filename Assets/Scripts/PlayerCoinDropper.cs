using UnityEngine;
using System.Collections;

public class PlayerCoinDropper : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private float spawnRadius = 1f;
    [SerializeField] private float burstForce = 3f;

    public void DropCoins(int amount, Vector2 origin)
{
    for (int i = 0; i < amount; i++)
    {
        Vector2 spawnPos = origin + Random.insideUnitCircle * spawnRadius;

        GameObject coinObj = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

        Coin coin = coinObj.GetComponent<Coin>();

        Vector2 dir = Random.insideUnitCircle.normalized;

        // ensure physics is initialized before force
        coin.StartCoroutine(ApplyImpulseNextFrame(coin, dir));
    }
}

private IEnumerator ApplyImpulseNextFrame(Coin coin, Vector2 dir)
{
    yield return null; // wait 1 frame

    coin.AddImpulse(dir * burstForce);
}
}
