using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
 
// Zentrale Steuerung fuer den "King of the Coin"-Modus.
// Liegt dieser Manager in der Szene, schalten Coin und PlayerCombat
// automatisch in den neuen Modus. Ohne Manager laeuft alles klassisch.
public class KingOfCoinManager : MonoBehaviour
{
    public static KingOfCoinManager Instance;
 
    [Header("Coin")]
    public GameObject coinPrefab;
    public Transform[] spawnPoints;
    [Tooltip("Wartezeit, bis nach einem Treffer ein neuer Coin erscheint.")]
    public float respawnDelay = 0.4f;
    [Tooltip("Neuen Coin dort spawnen, wo der Traeger getroffen wurde. Sonst: zufaelliger Spawnpunkt.")]
    public bool spawnAtDropPosition = false;
 
    [Header("Krone")]
    public GameObject crown;
    public Vector3 crownOffset = new Vector3(0f, 1.5f, 0f);
 
    [Header("Sieg-Bedingungen")]
    [Tooltip("Wer als Erster so viele Sekunden Gesamtzeit erreicht, gewinnt sofort.")]
    public float targetSeconds = 60f;
    [Tooltip("Match-Dauer in Sekunden (600 = 10 Minuten). Danach gewinnt die hoechste Gesamtzeit.")]
    public float matchDuration = 600f;
 
    [Header("UI (optional)")]
    public TMP_Text carrierTimerText;   
    public TMP_Text matchTimerText;    
    [Tooltip("Wenn an: die vorhandenen Eck-Punkte-Felder zeigen die Zeit jedes Spielers.")]
    public bool showTimeOnPlayerPanels = true;
 
    [Header("Win Screen")]
    public GameObject winPanel;
    public TextMeshProUGUI winText;
 
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip winSound;
 
    private KingOfCoinPlayer currentCarrier;
    private List<KingOfCoinPlayer> players = new List<KingOfCoinPlayer>();
    private bool allRegistered = false;
    private float matchTimeLeft;
    private bool matchOver = false;
    private int lastSpawnIndex = -1;
    private int expectedPlayers = 0;
 
    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
    }
 
    void Start()
    {
        matchTimeLeft = matchDuration;
        expectedPlayers = PlayerSessionData.players.Count;
 
        if (crown != null) crown.SetActive(false);
        if (winPanel != null) winPanel.SetActive(false);
 
        StartCoroutine(SpawnFirstCoin());
    }
 
    void Update()
    {
        if (matchOver) return;
 
        EnsurePlayersRegistered();
 
        matchTimeLeft -= Time.deltaTime;
        if (matchTimeLeft <= 0f)
        {
            matchTimeLeft = 0f;
            EndMatch();
            return;
        }
 
        if (currentCarrier != null)
        {
            currentCarrier.accumulatedTime += Time.deltaTime;
 
            if (currentCarrier.accumulatedTime >= targetSeconds)
            {
                EndMatch();
                return;
            }
        }
 
        UpdateCrown();
        UpdateHud();
        UpdateScoreboard();
    }
 
   
 
    private void EnsurePlayersRegistered()
    {
        if (allRegistered) return;
 
        var movers = Object.FindObjectsByType<PlayerMovement>(FindObjectsSortMode.None);
        if (expectedPlayers > 0 && movers.Length < expectedPlayers) return;
 
        players.Clear();
        foreach (var m in movers)
        {
            var kp = m.GetComponent<KingOfCoinPlayer>();
            if (kp == null) kp = m.gameObject.AddComponent<KingOfCoinPlayer>();
            players.Add(kp);
        }
 
        if (expectedPlayers == 0 || players.Count >= expectedPlayers)
            allRegistered = true;
    }
 
    // Coin: Aufnehmen & Abwerfen
 
    public void OnCoinPickedUp(GameObject playerObj)
    {
        if (matchOver || playerObj == null) return;
 
        var kp = playerObj.GetComponent<KingOfCoinPlayer>();
        if (kp == null) kp = playerObj.AddComponent<KingOfCoinPlayer>();
 
        SetCarrier(kp);
    }
 
    // Wird von PlayerCombat aufgerufen, wenn der Traeger getroffen wird.
    public void DropCoin(KingOfCoinPlayer kp)
    {
        if (matchOver || kp == null || kp != currentCarrier) return;
 
        Vector3 dropPos = kp.transform.position;
 
        currentCarrier.SetCarrier(false);
        currentCarrier = null;
        if (crown != null) crown.SetActive(false);
 
        Debug.Log("[KotC] Coin abgeworfen - neuer Coin in " + respawnDelay + "s");
        StartCoroutine(RespawnCoin(dropPos));
    }
 
    private void SetCarrier(KingOfCoinPlayer kp)
    {
        if (currentCarrier != null && currentCarrier != kp)
            currentCarrier.SetCarrier(false);
 
        currentCarrier = kp;
        currentCarrier.SetCarrier(true);
        if (crown != null) crown.SetActive(true);
    }
 
    // Coin spawnen 
 
    private IEnumerator SpawnFirstCoin()
    {
        yield return null;
        SpawnCoinAtRandomPoint();
    }
 
    private IEnumerator RespawnCoin(Vector3 dropPos)
    {
        yield return new WaitForSeconds(respawnDelay);
        if (matchOver) yield break;
 
        if (spawnAtDropPosition) SpawnCoin(dropPos);
        else SpawnCoinAtRandomPoint();
    }
 
    private void SpawnCoinAtRandomPoint()
    {
        Vector3 pos = transform.position;
 
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int index = 0;
            if (spawnPoints.Length > 1)
            {
                do { index = Random.Range(0, spawnPoints.Length); }
                while (index == lastSpawnIndex);
            }
            lastSpawnIndex = index;
            if (spawnPoints[index] != null) pos = spawnPoints[index].position;
        }
 
        SpawnCoin(pos);
    }
 
    private void SpawnCoin(Vector3 pos)
    {
        if (coinPrefab == null)
        {
            Debug.LogWarning("[KotC] Kein Coin Prefab zugewiesen!");
            return;
        }
 
        GameObject coinObj = Instantiate(coinPrefab, pos, Quaternion.identity);
        Coin coin = coinObj.GetComponent<Coin>();
        if (coin != null)
        {
            coin.kingMode = true;
            coin.regularSpawn = false;
        }
        Debug.Log("[KotC] Neuer Coin gespawnt bei " + pos);
    }
 
    
 
    private void UpdateCrown()
    {
        if (crown == null) return;
 
        if (currentCarrier == null)
        {
            crown.SetActive(false);
            return;
        }
 
        crown.SetActive(true);
 
        float gravityDir = 1f;
        var movement = currentCarrier.GetComponent<PlayerMovement>();
        if (movement != null && movement.rb != null)
            gravityDir = movement.rb.gravityScale > 0 ? 1f : -1f;
 
        Vector3 adjusted = new Vector3(crownOffset.x, crownOffset.y * gravityDir, crownOffset.z);
        crown.transform.position = currentCarrier.transform.position + adjusted;
 
        Vector3 s = crown.transform.localScale;
        s.y = Mathf.Abs(s.y) * gravityDir;
        crown.transform.localScale = s;
    }
 
    private void UpdateHud()
    {
        if (matchTimerText != null)
        {
            int total = Mathf.CeilToInt(matchTimeLeft);
            int min = total / 60;
            int sec = total % 60;
            matchTimerText.text = $"{min:0}:{sec:00}";
        }
 
        if (carrierTimerText != null)
        {
            if (currentCarrier != null)
            {
                int id = GetPlayerId(currentCarrier.gameObject);
                int t = Mathf.FloorToInt(currentCarrier.accumulatedTime);
                carrierTimerText.text = $"Player {id} owns the coin";
            }
            else
            {
                carrierTimerText.text = "Collect Coin";
            }
        }
    }
 
    
    private void UpdateScoreboard()
    {
        if (!showTimeOnPlayerPanels) return;
 
        foreach (var p in players)
        {
            if (p == null) continue;
            var inv = p.GetComponent<PlayerInventory>();
            if (inv != null && inv.scoreTextField != null)
            {
                inv.scoreTextField.text = Mathf.FloorToInt(p.accumulatedTime) + "s";
            }
        }
    }
 
    private int GetPlayerId(GameObject playerObj)
    {
        var input = playerObj.GetComponent<PlayerInput>();
        return input != null ? input.playerIndex + 1 : 0;
    }
 
    // Match-Ende 
 
    private void EndMatch()
    {
        if (matchOver) return;
        matchOver = true;
 
        if (crown != null) crown.SetActive(false);
 
        if (audioSource != null && winSound != null)
            audioSource.PlayOneShot(winSound);
 
        if (winPanel != null) winPanel.SetActive(true);
 
        DisplayRanking();
 
        Time.timeScale = 0f;
    }
 
    private void DisplayRanking()
    {
        if (winText == null) return;
 
        var sorted = players.Where(p => p != null)
                            .OrderByDescending(p => p.accumulatedTime)
                            .ToList();
 
        string ranking = "";
        for (int i = 0; i < sorted.Count; i++)
        {
            int id = GetPlayerId(sorted[i].gameObject);
            int t = Mathf.FloorToInt(sorted[i].accumulatedTime);
            string rank = (i + 1) + ".";
            ranking += $"{rank}<pos=15%>Spieler {id}:<pos=75%>{t}s\n";
        }
 
        winText.text = ranking;
    }
}