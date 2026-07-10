using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;
    public TextMeshProUGUI winText;

    [Header("Pause Menu")]
    public GameObject pausePanel;
    public GameObject resumeButton;
    private bool gameIsPaused = false;

    [Header("Start-Countdown")]
    public TMP_Text countdownText;         
    public int countdownSeconds = 3;
    public string goText = "Los!";

   
    public static event System.Action OnRoundStart;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
    [Tooltip("Komplette Countdown-Aufnahme ")]
    public AudioClip countdownVoice;        
    public AudioClip goSound;             

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(StartRoundCountdown());
        
        if (MusicManager.Instance == null)
        {
            Debug.LogError("Kein MusicManager in der Szene gefunden.");
            return;
        }

        MusicManager.Instance.PlayGameMusic();
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

 

    private IEnumerator StartRoundCountdown()
    {
      
        yield return null;

        LockAllPlayers(true);

        if (countdownText != null) countdownText.gameObject.SetActive(true);

        
        if (audioSource != null && countdownVoice != null)
            audioSource.PlayOneShot(countdownVoice);

        for (int i = countdownSeconds; i > 0; i--)
        {
            if (countdownText != null) countdownText.text = i.ToString();

        
            float t = 0f;
            while (t < 1f)
            {
                LockAllPlayers(true);
                t += Time.deltaTime;
                yield return null;
            }
        }

        if (countdownText != null) countdownText.text = goText;
        if (audioSource != null && goSound != null)
            audioSource.PlayOneShot(goSound);

       
        LockAllPlayers(false);
        OnRoundStart?.Invoke();

        yield return new WaitForSeconds(0.6f);
        if (countdownText != null) countdownText.gameObject.SetActive(false);
    }

    private void LockAllPlayers(bool locked)
    {
        var players = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        foreach (var p in players)
            p.SetControlsEnabled(!locked);
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