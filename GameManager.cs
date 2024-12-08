using UnityEngine;
using UnityEngine.SceneManagement;  // Sahne yönetimi için gerekli
using UnityEngine.UI;               // UI elemanlarý için gerekli

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pausePanel;    // Pause panel referansý
    public GameObject gameOverPanel; // Game over panel referansý
    public Button continueButton;    // Oyuna devam butonu
    public Button mainMenuButton;    // Ana menü butonu
    public Button restartButton;     // Oyunu yeniden baþlat butonu
    public Button gameOverMainMenuButton;  // GameOver panelindeki ana menü butonu

    private bool isPaused = false;   // Oyunun durdurulup durdurulmadýðýný takip eder

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Paneller baþlangýçta kapalý
        pausePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // Buton týklama eventlerini atama
        continueButton.onClick.AddListener(ResumeGame);
        mainMenuButton.onClick.AddListener(GoToMainMenu);
        restartButton.onClick.AddListener(RestartGame);
        gameOverMainMenuButton.onClick.AddListener(GoToMainMenu);
    }

    private void Update()
    {
        // Escape tuþuna basýlýrsa oyunu durdur veya devam ettir
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // Oyunu durdur ve paneli göster
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0;  // Oyunu durdur
        pausePanel.SetActive(true);  // Pause panelini göster
    }

    // Oyunu devam ettir ve paneli gizle
    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1;  // Oyunu devam ettir
        pausePanel.SetActive(false);  // Paneli gizle
    }

    // Game Over olduðunda GameOver panelini göster
    public void GameOver()
    {
        Time.timeScale = 0;  // Oyun durduruluyor
        gameOverPanel.SetActive(true);  // GameOver panelini aç

        // En yüksek skoru ve o oyunda alýnan puaný göster
        ScoreManager.Instance.ShowGameOverScores();

    }

    // Ana menüye dön
    public void GoToMainMenu()
    {
        Time.timeScale = 1;  // Oyun durdurulduysa devam ettir
        SceneManager.LoadScene("MainScene");  // Ana menü sahnesini yükle
    }

    // Oyunu yeniden baþlat
    public void RestartGame()
    {
        Time.timeScale = 1;  // Oyun tekrar devam etmeli
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Mevcut sahneyi yeniden yükle
    }
}
