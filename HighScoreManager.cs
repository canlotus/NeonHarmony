using UnityEngine;
using System.Collections.Generic;

public class HighScoreManager : MonoBehaviour
{
    private const int MaxHighScores = 10; // Maksimum 10 skor saklanacak
    private const string HighScoreKeyPrefix = "HighScore"; // Skor anahtarı prefiks

    public static HighScoreManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeHighScores(); // Varsayılan skorları yükle
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Varsayılan olarak 10 skor ekle
    private void InitializeHighScores()
    {
        for (int i = 0; i < MaxHighScores; i++)
        {
            if (!PlayerPrefs.HasKey($"{HighScoreKeyPrefix}{i}"))
            {
                PlayerPrefs.SetInt($"{HighScoreKeyPrefix}{i}", 0);
            }
        }
        PlayerPrefs.Save();
    }

    // Yeni skoru ekle ve sırala
    public void AddScore(int score)
    {
        List<int> highScores = GetHighScores();

        // Skoru listeye ekle
        highScores.Add(score);

        // Büyükten küçüğe sırala
        highScores.Sort((a, b) => b.CompareTo(a));

        // İlk 10 skoru kaydet
        for (int i = 0; i < MaxHighScores; i++)
        {
            if (i < highScores.Count)
            {
                PlayerPrefs.SetInt($"{HighScoreKeyPrefix}{i}", highScores[i]);
            }
            else
            {
                PlayerPrefs.SetInt($"{HighScoreKeyPrefix}{i}", 0); // Boş yerleri 0 yap
            }
        }

        PlayerPrefs.Save();
    }

    // Skorları al
    public List<int> GetHighScores()
    {
        List<int> highScores = new List<int>();

        for (int i = 0; i < MaxHighScores; i++)
        {
            highScores.Add(PlayerPrefs.GetInt($"{HighScoreKeyPrefix}{i}", 0));
        }

        return highScores;
    }
}