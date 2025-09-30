using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerScript player;
    public GameObject playerObject;
    public Text scoreText;
    public Text countdownText;
    public Text highScoreText;  

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playButton;

    private int score;
    [SerializeField] private int highScore;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ShowMainMenu();
    }

    // Waiting screen
    public void ShowMainMenu()
    {
        score = 0;
        scoreText.text = "0";
        highScoreText.text = "Best: " + highScore.ToString(); // hiển thị highscore

        playerObject.SetActive(false);   // tắt player
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);
    }

    // Gameplay
    public void PlayGame()
    {
        score = 0;
        scoreText.text = score.ToString();

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        playButton.SetActive(false);

        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();
        foreach (var pipe in pipes)
            Destroy(pipe.gameObject);

        playerObject.SetActive(true);
        player.enabled = false;

        StartCoroutine(CountdownAndStart());
    }

    private IEnumerator CountdownAndStart()
    {
        int countdown = 3;
        countdownText.gameObject.SetActive(true);

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1f); // không bị ảnh hưởng bởi Time.timeScale
            countdown--;
        }
        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1;
        player.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Gameover
    public void GameOver()
    {
        player.enabled = false;
        Time.timeScale = 0;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore); // lưu vào bộ nhớ
            PlayerPrefs.Save();
        }

        highScoreText.text = "Best: " + highScore.ToString(); // luôn hiển thị best score

        gameOverPanel.SetActive(true);
        playButton.SetActive(true);
    }

    // Score
    public void Scoring()
    {
        score++;
        scoreText.text = score.ToString();
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxPoint);
        }
    }

    // quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
