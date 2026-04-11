using UnityEngine;
using UnityEngine.InputSystem;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Das ECHTE Spieler-Prefab (mit Physik, Waffen etc.)")]
    public GameObject playerPrefab;

    void Start()
    {
        
        if (PlayerSessionData.keyboardLeftJoined)
        {
            PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyBoardLeft", pairWithDevice: Keyboard.current);
        }

        if (PlayerSessionData.keyboardRightJoined)
        {
            PlayerInput.Instantiate(playerPrefab, controlScheme: "KeyBoardRight", pairWithDevice: Keyboard.current);
        }

        foreach (var gamepad in PlayerSessionData.joinedGamepads)
        {
            PlayerInput.Instantiate(playerPrefab, controlScheme: "Gamepad", pairWithDevice: gamepad);
        }
    }
}