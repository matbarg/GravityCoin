using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class LeaderManager : MonoBehaviour
{
    public TextMeshProUGUI leaderText; 
    public GameObject crown;           
    public Vector3 crownOffset = new Vector3(0, 1.5f, 0); 

    private PlayerInventory[] allPlayerInventories;
    private PlayerInventory currentLeader;

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
        PlayerInventory highestScorer = null;
        int maxCoins = 0;

        // 1. Wer hat aktuell die meisten Münzen?
        foreach (var inventory in allPlayerInventories)
        {
            if (inventory != null && inventory.coins > maxCoins)
            {
                maxCoins = inventory.coins;
                highestScorer = inventory;
            }
        }


        if (currentLeader != null && currentLeader.coins == maxCoins && maxCoins > 0)
        {
            highestScorer = currentLeader;
        }

        // 3. Den neuen Leader festlegen
        currentLeader = highestScorer;

        if (currentLeader != null && maxCoins > 0)
        {
            crown.SetActive(true);

            // Gravitations-Check
            PlayerMovement movement = currentLeader.GetComponent<PlayerMovement>();
            float gravityDir = 1f;

            if (movement != null && movement.rb != null)
            {
                gravityDir = movement.rb.gravityScale > 0 ? 1f : -1f; 
            }

            // Krone positionieren
            Vector3 adjustedOffset = new Vector3(crownOffset.x, crownOffset.y * gravityDir, crownOffset.z);
            crown.transform.position = currentLeader.transform.position + adjustedOffset;
            
            // Krone drehen
            Vector3 crownScale = crown.transform.localScale;
            crownScale.y = Mathf.Abs(crownScale.y) * gravityDir;
            crown.transform.localScale = crownScale;
            
            // Text anzeigen
            var input = currentLeader.GetComponent<PlayerInput>();
            if (input != null)
            {
                int playerID = input.playerIndex + 1;
                leaderText.text = "Spieler " + playerID + " führt!";
            }
        }
        else
        {
            crown.SetActive(false);
            leaderText.text = "Sammelt Coins!";
            currentLeader = null;
        }
    }
}