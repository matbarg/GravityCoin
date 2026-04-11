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
        // 1. Tastatur Links (W)
        if (!PlayerSessionData.keyboardLeftJoined && Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
        {
            PlayerSessionData.keyboardLeftJoined = true;
            ActivateUI(0);
            Debug.Log("P1 (WASD) ist bereit!");
        }

        // 2. Tastatur Rechts (Pfeile)
        if (!PlayerSessionData.keyboardRightJoined && Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            PlayerSessionData.keyboardRightJoined = true;
            ActivateUI(1); 
            Debug.Log("P2 (Pfeile) ist bereit!");
        }

        // 3. Controller
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                if (!PlayerSessionData.joinedGamepads.Contains(gamepad))
                {
                    PlayerSessionData.joinedGamepads.Add(gamepad);
                    int slotIndex = 2 + (PlayerSessionData.joinedGamepads.Count - 1);
                    ActivateUI(slotIndex);
                    Debug.Log("Controller Spieler bereit!");
                }
            }
        }
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
        // Trage hier exakt den Namen deiner echten Level-Szene ein (z.B. "World1")
        SceneManager.LoadScene("World1"); 
    }
}
