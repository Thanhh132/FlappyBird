using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerScript player;
    public GameObject playerObject;
    public GameObject spawner;

    public Text scoreText;
    public Text countdownText;
    public Text highScoreText;

    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject restartHighScoreButton;
    [SerializeField] private GameObject quitButton;

    private int score;
    [SerializeField] private int highScore;

    private bool isPaused = false;
    private bool isGameOver = false;
    private bool isCountingDown = false;

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        score = 0;
        scoreText.text = "0";
        highScoreText.text = "BestScore: " + highScore.ToString();

        playerObject.SetActive(false);
        spawner.SetActive(false);

        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        pauseButton.SetActive(false);
        playButton.SetActive(true);
        restartHighScoreButton.SetActive(true);

        Time.timeScale = 1;
        isPaused = false;
        isGameOver = false;
        isCountingDown = false;
    }

    public void PlayGame()
    {
        playButton.SetActive(false);

        if (isGameOver)
        {
            player.transform.position = Vector3.zero;
            player.transform.rotation = Quaternion.identity;
            player.enabled = false;
        }

        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        restartHighScoreButton.SetActive(false);

        if (isPaused)
        {
            StartCoroutine(CountdownAndResume());
            return;
        }

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        foreach (var pipe in pipes)
            Destroy(pipe.gameObject);

        playerObject.SetActive(true);
        spawner.SetActive(true);
        player.enabled = false;

        score = 0;
        scoreText.text = "0";
        isGameOver = false;

        StartCoroutine(CountdownAndStart());
    }

    private IEnumerator CountdownAndStart()
    {
        if (isCountingDown) yield break;
        isCountingDown = true;

        Time.timeScale = 0;
        int countdown = 3;
        countdownText.gameObject.SetActive(true);

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);

        Time.timeScale = 1;
        player.enabled = true;
        pauseButton.SetActive(true);
        isPaused = false;

        isCountingDown = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator CountdownAndResume()
    {
        if (isCountingDown) yield break;
        isCountingDown = true;

        Time.timeScale = 0;
        int countdown = 3;
        countdownText.gameObject.SetActive(true);

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }

        countdownText.gameObject.SetActive(false);
        Time.timeScale = 1;
        pauseButton.SetActive(true);
        isPaused = false;

        isCountingDown = false;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void Scoring()
    {
        score++;
        scoreText.text = score.ToString();
        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxPoint);
    }

    public void GameOver()
    {
        player.enabled = false;
        Time.timeScale = 0;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        highScoreText.text = "BestScore: " + highScore.ToString();

        isGameOver = true;

        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(true);
    }

    public void Pause()
    {
        if (isPaused || isCountingDown) return;

        Time.timeScale = 0;
        isPaused = true;

        pauseButton.SetActive(false);
        playButton.SetActive(true); 
    }

    public void RestartHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScore = 0;
        highScoreText.text = "BestScore: 0";
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
