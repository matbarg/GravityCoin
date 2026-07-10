using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Spieler-Prefabs")]
    public GameObject[] playerPrefabs;
    public Transform[] spawnPoints;

    [Header("UI Textfelder (Index 0 = Spieler 1, etc.)")]
    public TextMeshProUGUI[] playerTextFields;

    [Header("Boost")]
    [Tooltip("Boost fuer diese Szene an/aus. In Earth-Szenen an, in KotC-Szenen aus.")]
    public bool enableBoost = true;
    [Tooltip("Boost-Ladebalken pro Spieler (Index 0 = Spieler 1). Fill-Images.")]
    public Image[] playerBoostBars;

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

        // --- Boost einrichten ---
        PlayerBoost boost = player.GetComponent<PlayerBoost>();
        if (boost != null)
        {
            boost.boostEnabled = enableBoost;

            if (playerBoostBars != null && index < playerBoostBars.Length)
                boost.boostBar = playerBoostBars[index];
        }
    }
}