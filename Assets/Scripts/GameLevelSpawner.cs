using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Spieler-Prefabs")]
    public GameObject[] playerPrefabs; 
    public Transform[] spawnPoints;

    [Header("UI Textfelder (Index 0 = Spieler 1, etc.)")]
    public TextMeshProUGUI[] playerTextFields;

    void Start()
    {
        for (int i = 0; i < PlayerSessionData.players.Count; i++)
        {
            Spawn(PlayerSessionData.players[i], i);
        }
    }

    void Spawn(PlayerInfo playerInfo, int index)
    {
        GameObject prefabToSpawn = playerPrefabs[index % playerPrefabs.Length];

        string scheme = "";
        InputDevice device = null;

        switch (playerInfo.inputType)
        {
            case InputType.KeyboardLeft:
                scheme = "KeyBoardLeft";
                device = Keyboard.current;
                break;

            case InputType.KeyboardRight:
                scheme = "KeyBoardRight";
                device = Keyboard.current;
                break;

            case InputType.Gamepad:
                scheme = "Gamepad";
                device = playerInfo.gamepad;
                break;
        }

        PlayerInput input = PlayerInput.Instantiate(
            prefabToSpawn,
            controlScheme: scheme,
            pairWithDevice: device
        );

        GameObject player = input.gameObject;

        // IMPORTANT: do NOT rely on input.playerIndex anymore
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
    }
}