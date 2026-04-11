using System.Collections.Generic;
using UnityEngine.InputSystem;


public static class PlayerSessionData
{
    public static bool keyboardLeftJoined = false;
    public static bool keyboardRightJoined = false;
    
    public static List<Gamepad> joinedGamepads = new List<Gamepad>();

    public static void ResetLobby()
    {
        keyboardLeftJoined = false;
        keyboardRightJoined = false;
        joinedGamepads.Clear();
    }
}