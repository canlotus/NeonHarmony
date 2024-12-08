using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;  // Singleton instance

    public TMP_Text scoreText;       // Toplam skoru gösteren text
    public TMP_Text pointsText;      // Anlık alınan puanı gösteren text
    public TMP_Text highScoreText;   // En yüksek skoru gösteren text (GameOver ekranında)
    public TMP_Text finalScoreText;  // Game Over ekranında toplam skoru göstermek için
    public TMP_Text placementText;   // Kaçıncı sırada olduğunu gösterecek text
    public AudioClip scoreSound;     // Oynatılacak ses dosyası

    private int totalScore = 0;      // Toplam skor
    private int highScore = 0;       // En yüksek skor

    private void Awake()
    {
        // Singleton instance ataması
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // PlayerPrefs'ten en yüksek skoru yükle
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Varsayılan değer 0
    }

    // Skoru arttır ve UI'yi güncelle
    public void AddScore(int scoreToAdd)
    {
        totalScore += scoreToAdd;
        UpdateScoreText();
        ShowPoints(scoreToAdd);

        SoundManager.Instance.PlaySound(scoreSound);

        if (totalScore > highScore)
        {
            highScore = totalScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    private void ShowPoints(int points)
    {
        pointsText.text = "+" + points.ToString();
        pointsText.gameObject.SetActive(true);
        StartCoroutine(HidePointsTextAfterDelay(1.0f));
    }

    private System.Collections.IEnumerator HidePointsTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        pointsText.gameObject.SetActive(false);
    }

    public void ShowGameOverScores()
    {
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        finalScoreText.text = "Your Score: " + totalScore.ToString();

        int placement = SaveScoreToTop10();
        SetPlacementText(placement);
    }

    private int SaveScoreToTop10()
    {
        const int maxScores = 10;
        string scoreKey = "HighScore";
        int[] topScores = new int[maxScores];

        for (int i = 0; i < maxScores; i++)
        {
            topScores[i] = PlayerPrefs.GetInt($"{scoreKey}{i}", 0);
        }

        topScores[maxScores - 1] = totalScore;
        System.Array.Sort(topScores, (a, b) => b.CompareTo(a));

        int placement = -1;

        for (int i = 0; i < maxScores; i++)
        {
            PlayerPrefs.SetInt($"{scoreKey}{i}", topScores[i]);

            if (topScores[i] == totalScore && placement == -1)
            {
                placement = i + 1;
            }
        }

        PlayerPrefs.Save();
        return placement;
    }

    public int[] GetTop10Scores()
    {
        const int maxScores = 10;
        int[] topScores = new int[maxScores];

        for (int i = 0; i < maxScores; i++)
        {
            topScores[i] = PlayerPrefs.GetInt($"HighScore{i}", 0);
        }

        return topScores;
    }

    private void SetPlacementText(int placement)
    {
        if (placement <= 0) return;

        string placementSuffix;
        Color placementColor;

        switch (placement)
        {
            case 1:
                placementSuffix = "1st";
                placementColor = new Color(1f, 0.84f, 0f); // Altın sarısı
                break;
            case 2:
                placementSuffix = "2nd";
                placementColor = new Color(0.75f, 0.75f, 0.75f); // Gümüş rengi
                break;
            case 3:
                placementSuffix = "3rd";
                placementColor = new Color(0.8f, 0.5f, 0.2f); // Bronz rengi
                break;
            default:
                placementSuffix = $"{placement}th";
                placementColor = new Color(0.6f, 0.6f, 0.6f); // Demir rengi
                break;
        }

        placementText.text = $"Your Rank  {placementSuffix}";
        placementText.color = placementColor;
    }
}