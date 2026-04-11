using System.Collections.Generic;
using UnityEngine.InputSystem;

public enum InputType
{
    KeyboardLeft,
    KeyboardRight,
    Gamepad
}

public class PlayerInfo
{
    public InputType inputType;
    public Gamepad gamepad; // null for keyboard players
}

public static class PlayerSessionData
{
    public static List<PlayerInfo> players = new List<PlayerInfo>();

    public static void ResetLobby()
    {
        players.Clear();
    }
}