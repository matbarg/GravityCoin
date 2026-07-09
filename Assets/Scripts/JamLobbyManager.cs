using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class JamLobbyManager : MonoBehaviour
{
    [Header("UI Astronauten (Join Logik)")]
    public GameObject[] characterModels;

    [Header("Map Auswahl UI")]
    public GameObject[] planetModels;
    public TextMeshProUGUI mapNameText;

    [Header("Map Daten")]
    public string[] sceneNames;
    public string[] displayNames;

    [TextArea] public string[] descriptions;
    public TMP_Text descriptionText;

    [TextArea] public string[] modusDescriptions;
    public TMP_Text modusDescriptionText;

    [Header("Map/Modus Aufteilung")]
    [Tooltip("Wie viele Maps es pro Modus gibt. Die Arrays sind sortiert: " +
             "erst ALLE Maps von Modus 0, dann ALLE Maps von Modus 1, usw. " +
             "Beispiel: World1, World3, World1_KingOfCoin, World3_KingOfCoin -> hier 2.")]
    public int mapsPerMode = 2;

    private int currentMapIndex = 0;   
    private int currentModeIndex = 0;   

   
    private int ModeCount => (mapsPerMode > 0) ? Mathf.Max(1, sceneNames.Length / mapsPerMode) : 1;

    
    private int FlatIndex => currentModeIndex * mapsPerMode + currentMapIndex;

    void Start()
    {
        PlayerSessionData.ResetLobby();

        foreach (var model in characterModels)
            if (model != null) model.SetActive(false);

        foreach (var planet in planetModels)
            if (planet != null) planet.SetActive(false);

        UpdateMapUI();
    }

    void Update()
    {
        CheckForJoinInput();
    }

  

    public void NextMap()
    {
        currentMapIndex = (currentMapIndex + 1) % mapsPerMode;
        UpdateMapUI();
    }

    public void PreviousMap()
    {
        currentMapIndex--;
        if (currentMapIndex < 0) currentMapIndex = mapsPerMode - 1;
        UpdateMapUI();
    }

  

    public void NextMode()
    {
        currentModeIndex = (currentModeIndex + 1) % ModeCount;
        UpdateMapUI();
    }

    public void PreviousMode()
    {
        currentModeIndex--;
        if (currentModeIndex < 0) currentModeIndex = ModeCount - 1;
        UpdateMapUI();
    }

  

    private void UpdateMapUI()
    {
        int idx = FlatIndex;

        // alle Planeten aus, nur den aktuellen an
        foreach (var planet in planetModels)
            if (planet != null) planet.SetActive(false);

        if (idx < planetModels.Length && planetModels[idx] != null)
            planetModels[idx].SetActive(true);

        if (mapNameText != null && idx < displayNames.Length)
            mapNameText.text = displayNames[idx];

        if (descriptionText != null && idx < descriptions.Length)
            descriptionText.text = descriptions[idx];

        if (modusDescriptionText != null && idx < modusDescriptions.Length)
            modusDescriptionText.text = modusDescriptions[idx];
    }

    

    bool AlreadyJoined(InputType type)
    {
        foreach (var player in PlayerSessionData.players)
            if (player.inputType == type) return true;
        return false;
    }

    bool AlreadyJoined(Gamepad pad)
    {
        foreach (var player in PlayerSessionData.players)
            if (player.gamepad == pad) return true;
        return false;
    }

    void AddPlayer(InputType type, Gamepad pad)
    {
        PlayerInfo newPlayer = new PlayerInfo
        {
            inputType = type,
            gamepad = pad
        };
        PlayerSessionData.players.Add(newPlayer);
        int slotIndex = PlayerSessionData.players.Count - 1;
        ActivateUI(slotIndex);
    }

    private void ActivateUI(int index)
    {
        if (index < characterModels.Length && characterModels[index] != null)
            characterModels[index].SetActive(true);
    }

    private void CheckForJoinInput()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardLeft))
            {
                AddPlayer(InputType.KeyboardLeft, null);
                Debug.Log("Keyboard Left (E) joined");
            }
        }

        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardRight))
            {
                AddPlayer(InputType.KeyboardRight, null);
                Debug.Log("Keyboard Right (M) joined");
            }
        }

        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.rightShoulder.wasPressedThisFrame)
            {
                if (!AlreadyJoined(gamepad))
                {
                    AddPlayer(InputType.Gamepad, gamepad);
                    Debug.Log("Gamepad (R1) joined");
                }
            }
        }
    }

    public void StartGame()
    {
        if (PlayerSessionData.players.Count < 2)
        {
            Debug.Log("Nicht genug Spieler!");
            return;
        }

        int idx = FlatIndex;
        if (sceneNames.Length > 0 && idx < sceneNames.Length)
        {
            SceneManager.LoadScene(sceneNames[idx]);
        }
        else
        {
            Debug.LogError("Fehler: Keine passende Scene fuer Map/Modus gefunden!");
        }
    }
}