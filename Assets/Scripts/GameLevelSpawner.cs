using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Spieler-Prefabs")]
    public GameObject[] playerPrefabs; 
    public UIManager uiManager;
    public Transform[] spawnPoints;

    [Header("UI Textfelder (Index 0 = Spieler 1, etc.)")]
    public TextMeshProUGUI[] playerTextFields;

    
    private int spawnedPlayersCount = 0;

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

        GameObject prefabToSpawn = playerPrefabs[spawnedPlayersCount % playerPrefabs.Length];

        PlayerInput input = PlayerInput.Instantiate(
            prefabToSpawn,
            controlScheme: scheme,
            pairWithDevice: device
        );

        GameObject player = input.gameObject;
        int index = input.playerIndex; 

        if (spawnPoints.Length > 0)
        {
            player.transform.position = spawnPoints[index % spawnPoints.Length].position;
        }

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();

        if (index < playerTextFields.Length && playerTextFields[index] != null)
        {
            inventory.scoreTextField = playerTextFields[index];
            inventory.scoreTextField.text = "0";
        }
        spawnedPlayersCount++;
    }
}