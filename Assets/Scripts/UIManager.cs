using UnityEngine;

public class UIManager : MonoBehaviour
{
    public PlayerScoreUI uiPrefab;
    public Canvas canvas;

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