using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;


public class JamLobbyManager : MonoBehaviour
{
    private PlayerInputManager manager;
    private bool keyboardLeftJoined = false;
    private bool keyboardRightJoined = false;


    public List<PlayerInput> players = new List<PlayerInput>();

    
    void Start()

    
    {
        manager = GetComponent<PlayerInputManager>();
     
    }

    void Update()
    {
        // 1. Check für Tastatur Links (WASD) -> Drücke 'W' zum Beitreten
        if (!keyboardLeftJoined && Keyboard.current != null && Keyboard.current.wKey.wasPressedThisFrame)
        {
            PlayerInput p = manager.JoinPlayer(-1, -1, "KeyBoardLeft", Keyboard.current);
            AddPlayerToList(p);

            keyboardLeftJoined = true;
            Debug.Log("Player 1 (Keyboard Left) joined!");
        }

        // 2. Check für Tastatur Rechts (Pfeile) -> Drücke 'UpArrow' zum Beitreten
        if (!keyboardRightJoined && Keyboard.current != null && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            PlayerInput p = manager.JoinPlayer(-1, -1, "KeyBoardRight", Keyboard.current);
            AddPlayerToList(p);
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
                    PlayerInput p = manager.JoinPlayer(-1, -1, "Gamepad", gamepad);
                    AddPlayerToList(p);
                    Debug.Log("Controller Player joined!");
                }
            }
        }
    }
    

    
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
    private void AddPlayerToList(PlayerInput pi)
    {
        if (pi != null)
        {
            players.Add(pi);
        }
    }


}