using UnityEngine;

public class Coin : MonoBehaviour
{
    public CoinSpawner spawner;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("coin collision");
        
        PlayerInventory inv = other.GetComponent<PlayerInventory>();

        if (inv != null)
        {
            inv.AddCoin();
            //spawner.OnCoinCollected();
            Destroy(gameObject);
        }
    }
}