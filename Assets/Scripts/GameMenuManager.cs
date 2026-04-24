using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public void QuitGame()
    {
      
        Application.Quit();

       
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Debug.Log("Das Spiel wurde beendet.");
    }
}