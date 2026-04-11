using TMPro;
using UnityEngine;

public class PlayerScoreUI : MonoBehaviour
{
    public TMP_Text scoreText;

    public void SetScore(int playerId, int coins)
    {
        scoreText.text = $"P{playerId}: {coins}";
    }
    
    public void SetCorner(int index)
    {
        RectTransform rt = GetComponent<RectTransform>();

        switch (index)
        {
            case 0: rt.anchorMin = rt.anchorMax = new Vector2(0, 1); rt.pivot = new Vector2(0, 1); break;
            case 1: rt.anchorMin = rt.anchorMax = new Vector2(1, 1); rt.pivot = new Vector2(1, 1); break;
            case 2: rt.anchorMin = rt.anchorMax = new Vector2(0, 0); rt.pivot = new Vector2(0, 0); break;
            case 3: rt.anchorMin = rt.anchorMax = new Vector2(1, 0); rt.pivot = new Vector2(1, 0); break;
        }

        rt.anchoredPosition = new Vector2(20, -20);
    }
}