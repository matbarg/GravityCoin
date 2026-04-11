using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public int coins = 0;
    
    [HideInInspector]
    public TextMeshProUGUI scoreTextField;

    public PlayerCoinDropper dropper;
	public PlayerScoreUI ui;

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
        if (ui != null)
		{
        	ui.SetScore(id, coins);
		}
        
        if (scoreTextField != null)
        {
            scoreTextField.text = coins.ToString(); 
            // Oder schöner: scoreTextField.text = $"Coins: {coins}";
        }

    	Debug.Log($"Spieler {id} hat jetzt {coins} Münzen.");
    }
}