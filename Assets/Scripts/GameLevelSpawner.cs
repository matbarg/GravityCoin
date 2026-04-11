using UnityEngine;
using UnityEngine.InputSystem;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Das ECHTE Spieler-Prefab (mit Physik, Waffen etc.)")]
    public GameObject playerPrefab;
    public UIManager uiManager;
    public Transform[] spawnPoints;
    void Start()
    {
        Spawn(PlayerSessionData.keyboardLeftJoined, "KeyBoardLeft", Keyboard.current);
        Spawn(PlayerSessionData.keyboardRightJoined, "KeyBoardRight", Keyboard.current);

        foreach (var gamepad in PlayerSessionData.joinedGamepads)
        {
            Spawn(true, "Gamepad", gamepad);
        }
    }

    void Spawn(bool condition, string scheme, InputDevice device)
    {
        if (!condition) return;

        PlayerInput input = PlayerInput.Instantiate(
            playerPrefab,
            controlScheme: scheme,
            pairWithDevice: device
        );

        GameObject player = input.gameObject;

        int index = input.playerIndex;

        if (spawnPoints.Length > 0)
        {
            player.transform.position = spawnPoints[index % spawnPoints.Length].position;
        }
        
        // create UI for this player
        PlayerScoreUI ui = uiManager.CreateUI(index);

        // connect UI → player
        player.GetComponent<PlayerInventory>().ui = ui;
    }
}