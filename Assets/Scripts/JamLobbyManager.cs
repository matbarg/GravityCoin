using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class JamLobbyManager : MonoBehaviour
{
    [Header("UI Astronauten (Die Bilder in den Boxen)")]
    public GameObject[] characterModels;

    void Start()
    {
    
        PlayerSessionData.ResetLobby();
        
       
        foreach (var model in characterModels)
        {
            if (model != null) model.SetActive(false);
        }
    }

    void Update()
    {
        // Keyboard Left (W)
        if (Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardLeft))
            {
                AddPlayer(InputType.KeyboardLeft, null);
                Debug.Log("Keyboard Left joined");
            }
        }

        // Keyboard Right (Arrow Up)
        if (Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            if (!AlreadyJoined(InputType.KeyboardRight))
            {
                AddPlayer(InputType.KeyboardRight, null);
                Debug.Log("Keyboard Right joined");
            }
        }

        // Gamepads
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (!AlreadyJoined(gamepad))
                {
                    AddPlayer(InputType.Gamepad, gamepad);
                    Debug.Log("Gamepad joined");
                }
            }
        }
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
    public void StartGame()
    {
        if (PlayerSessionData.players.Count < 2)
        {
            Debug.Log("Spiel kann nicht starten: Es müssen mindestens 2 Spieler beitreten!");
            
            return; 
        }
        SceneManager.LoadScene("World1"); 
    }
}
