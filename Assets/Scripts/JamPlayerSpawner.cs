using UnityEngine;
using UnityEngine.InputSystem;

public class JamLobbyManager : MonoBehaviour
{
    private PlayerInputManager manager;
    private bool keyboardLeftJoined = false;
    private bool keyboardRightJoined = false;

    void Start()
    {
        manager = GetComponent<PlayerInputManager>();
     
    }

    void Update()
    {
        // 1. Check für Tastatur Links (WASD) -> Drücke 'W' zum Beitreten
        if (!keyboardLeftJoined && Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
        {
            manager.JoinPlayer(-1, -1, "KeyBoardLeft", Keyboard.current);
            keyboardLeftJoined = true;
            Debug.Log("Player 1 (Keyboard Left) joined!");
        }

        // 2. Check für Tastatur Rechts (Pfeile) -> Drücke 'UpArrow' zum Beitreten
        if (!keyboardRightJoined && Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            manager.JoinPlayer(-1, -1, "KeyBoardRight", Keyboard.current);
            keyboardRightJoined = true;
            Debug.Log("Player 2 (Keyboard Right) joined!");
        }

        // 3. Check für Controller -> Drücke den 'Süd-Button' (A / X)
        foreach (var gamepad in Gamepad.all)
        {
            if (gamepad.buttonSouth.wasPressedThisFrame)
            {
                // Wir prüfen, ob dieser Controller schon vergeben ist
                if (!IsDeviceAlreadyUsed(gamepad))
                {
                    manager.JoinPlayer(-1, -1, "Gamepad", gamepad);
                    Debug.Log("Controller Player joined!");
                }
            }
        }
    }

    // Hilfsfunktion: Prüft, ob ein Gerät schon einem Spieler gehört
    private bool IsDeviceAlreadyUsed(InputDevice device)
    {
        foreach (var player in PlayerInput.all)
        {
            foreach (var d in player.devices)
            {
                if (d == device) return true;
            }
        }
        return false;
    }
}