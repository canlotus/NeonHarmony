using UnityEngine;
using TMPro;

public class HighScoreDisplay : MonoBehaviour
{
    public GameObject highScorePanel; // Panel referansı
    public TMP_Text[] scoreTexts;    // Skorları gösterecek text alanları

    private void Start()
    {
        highScorePanel.SetActive(false);
    }

    public void OpenHighScorePanel()
    {
        highScorePanel.SetActive(true);
        ShowHighScores();
    }

    public void CloseHighScorePanel()
    {
        highScorePanel.SetActive(false);
    }

    public void ShowHighScores()
    {
        var highScores = HighScoreManager.Instance.GetHighScores();

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < highScores.Count && highScores[i] > 0)
            {
                scoreTexts[i].text = $"{i + 1}. {highScores[i]}";
            }
            else
            {
                scoreTexts[i].text = $"{i + 1}. ---";
            }
        }
    }
}