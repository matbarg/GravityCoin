using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class GameLevelSpawner : MonoBehaviour
{
    [Header("Das ECHTE Spieler-Prefab (mit Physik, Waffen etc.)")]
    public GameObject playerPrefab;
    public UIManager uiManager;

    [Header("UI Textfelder (Index 0 = Spieler 1, etc.)")]
    public TextMeshProUGUI[] playerTextFields;

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

        PlayerInventory inventory = input.gameObject.GetComponent<PlayerInventory>();

        if (index < playerTextFields.Length && playerTextFields[index] != null)
        {
            inventory.scoreTextField = playerTextFields[index];
            
            // Optional: Initialen Wert anzeigen
            inventory.scoreTextField.text = "0";
        }
    }
}