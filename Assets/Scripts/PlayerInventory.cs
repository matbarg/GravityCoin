using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;
    //public PlayerCoinDropper dropper;

    public void AddCoin(int amount = 1)
    {
        coins += amount;
        UpdateUI();
    }

    public void LoseCoins(int amount)
    {
        int lost = Mathf.Min(amount, coins);
        coins -= lost;

        //dropper.DropCoins(lost, transform.position);

        UpdateUI();
    }

    private void UpdateUI()
    {

        int id = GetComponent<UnityEngine.InputSystem.PlayerInput>().playerIndex;
        Debug.Log("Spieler " + id + " hat jetzt " + coins + " Münzen.");
        
        Debug.Log("Coins: " + coins);
        // UI update here
    }
}