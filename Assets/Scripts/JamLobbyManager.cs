using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class JamLobbyManager : MonoBehaviour
{
    [Header("UI Astronauten (Join Logik)")]
    public GameObject[] characterModels;

    [Header("Map Auswahl UI")] public GameObject[] planetModels;
    public TextMeshProUGUI mapNameText;

    [Header("Map Daten")] public string[] sceneNames;
    public string[] displayNames;
    private int currentMapIndex = 0;
    
    [TextArea] public string[] descriptions;
    public TMP_Text descriptionText;
    
    [TextArea] public string[] modusDescriptions;
    public TMP_Text modusDescriptionText; 

    void Start()
    {
        PlayerSessionData.ResetLobby();

        foreach (var model in characterModels)
        {
            if (model != null) model.SetActive(false);
        }
        foreach (var planet in planetModels)
        {
            if (planet != null) planet.SetActive(false);
        }

        UpdateMapUI();
    }

    void Update()
    {

        CheckForJoinInput();
        
    }

    public void NextMap()
    {
        // Aktuellen Planeten ausschalten
        planetModels[currentMapIndex].SetActive(false);

        // Index erhöhen (und am Ende wieder bei 0 starten)
        currentMapIndex = (currentMapIndex + 1) % sceneNames.Length;

        UpdateMapUI();
    }

    public void PreviousMap()
    {
        // Aktuellen Planeten ausschalten
        planetModels[currentMapIndex].SetActive(false);

        // Index verringern (und am Anfang wieder zum Ende springen)
        currentMapIndex--;
        if (currentMapIndex < 0) currentMapIndex = sceneNames.Length - 1;

        UpdateMapUI();
    }

    private void UpdateMapUI()
    {
        // Neuen Planeten aktivieren
        if (currentMapIndex < planetModels.Length)
            planetModels[currentMapIndex].SetActive(true);

        // Text aktualisieren
        if (mapNameText != null && currentMapIndex < displayNames.Length)
            mapNameText.text = displayNames[currentMapIndex];
        
        if (descriptionText != null && currentMapIndex < descriptions.Length)
            descriptionText.text = descriptions[currentMapIndex];
        
        if (modusDescriptionText != null && currentMapIndex < modusDescriptions.Length)
            modusDescriptionText.text = modusDescriptions[currentMapIndex];
    }

    bool AlreadyJoined(InputType type)
    {
        foreach (var player in PlayerSessionData.players)
        {
            if (player.inputType == type)
                return true;
        }

        return false;
    }

    bool AlreadyJoined(Gamepad pad)
    {
        foreach (var player in PlayerSessionData.players)
        {
            if (player.gamepad == pad)
                return true;
        }

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
        {
            characterModels[index].SetActive(true);
        }
    }

    private void CheckForJoinInput()
    {
        // Keyboard Links: E
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardLeft))
            {
                AddPlayer(InputType.KeyboardLeft, null);
                Debug.Log("Keyboard Left (E) joined");
            }
        }

        // Keyboard Rechts: M
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardRight))
            {
                AddPlayer(InputType.KeyboardRight, null);
                Debug.Log("Keyboard Right (M) joined");
            }
        }

        // Gamepads: R1 zum Joinen
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

        if (sceneNames.Length > 0 && currentMapIndex < sceneNames.Length)
        {
            string sceneToLoad = sceneNames[currentMapIndex];
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            Debug.LogError("Fehler: Du hast keine Scene Names im Inspector eingetragen!");
        }
    }
}