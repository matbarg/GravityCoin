using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    // Das "Singleton"-Pattern: Ermöglicht Zugriff via GameManager.Instance
    public static GameManager Instance; 

    public GameObject winPanel;
    public TextMeshProUGUI winText;
    
    [Header("Pause Menu")]
    public GameObject pausePanel;         
    public GameObject resumeButton;       
    private bool gameIsPaused = false;    
    

    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip winSound;

    void Awake()
    {
        // Initialisierung des Singletons
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
    
            if (winPanel != null && !winPanel.activeSelf) 
            {
                if (gameIsPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }
    }
    
    
    public void ResumeGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
            Time.timeScale = 1f;
            gameIsPaused = false;
        }
    }
    
    void PauseGame()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
            Time.timeScale = 0f;
            gameIsPaused = true;

            // Zwingt das EventSystem, den Resume-Button zu markieren (für Gamepad)
            if (resumeButton != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(resumeButton);
            }
        }
    }

    public void ShowWinScreen(int winnerID)
    {
        if (winPanel != null)
        {
            if (audioSource != null && winSound != null)
                audioSource.PlayOneShot(winSound);

            winPanel.SetActive(true);
            Time.timeScale = 0f;
            
            DisplayRanking();
        }
    }
    void DisplayRanking()
    {
    
        List<PlayerInventory> players = Object.FindObjectsByType<PlayerInventory>(FindObjectsSortMode.None).ToList();
        var sortedPlayers = players.OrderByDescending(p => p.coins).ToList();

  
        string rankingString = ""; 

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            int pID = sortedPlayers[i].GetComponent<UnityEngine.InputSystem.PlayerInput>().playerIndex + 1;
            int score = sortedPlayers[i].coins;

       
            string rank = (i + 1) + ".";
            rankingString += $"{rank}<pos=15%>Spieler {pID}:<pos=75%>{score} Coins\n";
        }

        winText.text = rankingString;
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