using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Das "Singleton"-Pattern: Ermöglicht Zugriff via GameManager.Instance
    public static GameManager Instance; 

    public GameObject winPanel;
    public TextMeshProUGUI winText;

    void Awake()
    {
        // Initialisierung des Singletons
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void ShowWinScreen(int playerIndex)
    {
        if (winPanel != null)
        {
            winPanel.SetActive(true);
            winText.text = $"Player {playerIndex + 1} wins!";
            Time.timeScale = 0f;
        }
    }

    public void Replay()
    {
        Debug.Log("button pressed");
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToMenu()
    {
        Debug.Log("button pressed");
        Time.timeScale = 1f;
        SceneManager.LoadScene("LobbyScene");
    }
}