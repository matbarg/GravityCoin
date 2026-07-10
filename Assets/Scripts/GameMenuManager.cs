using UnityEngine;

public class GameMenuManager : MonoBehaviour
{
    public void Start()
    {
        if (MusicManager.Instance == null)
        {
            Debug.LogError("Kein MusicManager in der Szene gefunden.");
            return;
        }

        MusicManager.Instance.PlayTitleMusic();
    }
    
    public void QuitGame()
    {
      
        Application.Quit();

       
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Debug.Log("Das Spiel wurde beendet.");
    }
}