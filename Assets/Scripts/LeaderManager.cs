using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LeaderManager : MonoBehaviour
{
    public TextMeshProUGUI leaderText; 
    public GameObject crown;           
    public Vector3 crownOffset = new Vector3(0, 1.5f, 0); 

    private PlayerInventory[] allPlayerInventories;

    void Update()
    {
       
        if (allPlayerInventories == null || allPlayerInventories.Length == 0)
        {
            allPlayerInventories = Object.FindObjectsByType<PlayerInventory>(FindObjectsSortMode.None);
        }

        DetermineLeader();
    }

    void DetermineLeader()
    {
        PlayerInventory bestInventory = null;
        int maxCoins = -1; 

        foreach (var inventory in allPlayerInventories)
        {
            if (inventory != null && inventory.coins > maxCoins)
            {
                maxCoins = inventory.coins;
                bestInventory = inventory;
            }
        }

        // Krone nur zeigen, wenn wirklich jemand führt UND mindestens 1 Coin hat
        if (bestInventory != null && maxCoins > 0)
        {
            crown.SetActive(true);
            crown.transform.position = bestInventory.transform.position + crownOffset;

            int playerID = bestInventory.GetComponent<PlayerInput>().playerIndex + 1;
            
            if (leaderText != null)
                leaderText.text = "Spieler " + playerID + " führt!";
        }
        else
        {
            crown.SetActive(false);
            if (leaderText != null) leaderText.text = "Sammelt Coins!";
        }
    }
}