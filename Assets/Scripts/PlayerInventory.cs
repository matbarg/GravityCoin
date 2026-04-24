using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;

    public int coins_for_win = 10;
    
    [HideInInspector]
    public TextMeshProUGUI scoreTextField;

    public PlayerCoinDropper dropper;

    public void AddCoin(int amount = 1)
    {
        coins += amount;
        UpdateUI();
    }

    public void LoseCoins(int amount)
    {
        int lost = Mathf.Min(amount, coins);
        coins -= lost;

        dropper.DropCoins(lost, transform.position);

        UpdateUI();
    }

    private void UpdateUI()
    {
        int id = GetComponent<UnityEngine.InputSystem.PlayerInput>().playerIndex;
        
        if (scoreTextField != null)
        {
            scoreTextField.text = coins.ToString(); 
        }

    	Debug.Log($"Spieler {id} hat jetzt {coins} Münzen.");

        if (coins >= coins_for_win)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.ShowWinScreen(id);
            }
            else
            {
                Debug.Log("GameManager nicht gefunden");
            }
        }
    }
}