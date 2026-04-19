using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerScoreUI uiPrefab;
    public Canvas canvas;
    public GameObject[] playerPanels;
    
    void Start()
    {
        foreach (var panel in playerPanels)
        {
            panel.SetActive(false);
        }
        
        int joinedPlayers = PlayerSessionData.players.Count;

        for (int i = 0; i < joinedPlayers; i++)
        {
            if (i < playerPanels.Length)
            {
                playerPanels[i].SetActive(true);
            }
        }
    }

    public PlayerScoreUI CreateUI(int playerIndex)
    {
        PlayerScoreUI ui = Instantiate(uiPrefab);
        ui.transform.SetParent(canvas.transform, false);

        RectTransform rt = ui.GetComponent<RectTransform>();
        rt.localScale = Vector3.one;

        ui.SetCorner(playerIndex);

        return ui;
    }
}