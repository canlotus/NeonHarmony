using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    public TMP_Text highScoreText;
    public TMP_Text transitionText;

    private void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore.ToString();

        if (transitionText != null)
        {
            StartCoroutine(FadeText());
        }
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("GameMode", 0);  // Normal mod
        SceneManager.LoadScene("GameScene");
    }

    public void StartBounceGame()
    {
        PlayerPrefs.SetInt("GameMode", 1);  // Bounciness mod
        SceneManager.LoadScene("GameScene");
    }

    public void StartMixedMode()
    {
        PlayerPrefs.SetInt("GameMode", 2);  // Karışık mod
        SceneManager.LoadScene("GameScene");
    }

    private IEnumerator FadeText()
    {
        float duration = 1f;
        Color originalColor = transitionText.color;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            originalColor.a = Mathf.Lerp(0, 1, normalizedTime);
            transitionText.color = originalColor;
            yield return null;
        }

        originalColor.a = 1;
        transitionText.color = originalColor;

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            originalColor.a = Mathf.Lerp(1, 0, normalizedTime);
            transitionText.color = originalColor;
            yield return null;
        }

        StartCoroutine(FadeText());
    }
}