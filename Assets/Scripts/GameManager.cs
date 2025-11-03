using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { Menu, Playing, Paused, GameOver, CountingDown }

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
    [SerializeField] private GameObject quitButton;

    private int score;
    [SerializeField] private int highScore;

    private GameState state = GameState.Menu;

    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        score = 0;
        scoreText.text = "0";
        highScoreText.text = "BestScore: " + highScore;

        playerObject.SetActive(false);
        spawner.SetActive(false);

        ApplyMenuUI();
        Time.timeScale = 1f;
        state = GameState.Menu;
    }

    public void PlayGame()
    {
        if (state == GameState.CountingDown) return;

        playButton.SetActive(false);
        gameOverPanel.SetActive(false);

        if (state == GameState.Paused)
        {
            StartCoroutine(DoCountdown(3, ResumeAfterPause));
            return;
        }

        NewGame();
    }

    public void Pause()
    {
        if (state != GameState.Playing) return;
        Time.timeScale = 0f;
        state = GameState.Paused;

        pauseButton.SetActive(false);
        playButton.SetActive(true);
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
        if (state == GameState.GameOver) return;

        player.enabled = false;
        Time.timeScale = 0f;

        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        highScoreText.text = "BestScore: " + highScore;

        ApplyGameOverUI();
        state = GameState.GameOver;
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    private void NewGame()
    {
        foreach (var pipe in FindObjectsOfType<Pipes>())
            Destroy(pipe.gameObject);

        playerObject.SetActive(true);
        spawner.SetActive(true);
        player.enabled = false;

        ResetRun();

        StartCoroutine(DoCountdown(3, StartPlaying));
    }

    private void ResumeAfterPause()
    {
        Time.timeScale = 1f;
        pauseButton.SetActive(true);
        state = GameState.Playing;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void StartPlaying()
    {
        Time.timeScale = 1f;
        player.enabled = true;
        ApplyPlayingUI();
        state = GameState.Playing;
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void ResetRun()
    {
        player.transform.position = Vector3.zero;
        player.transform.rotation = Quaternion.identity;

        score = 0;
        scoreText.text = "0";
    }


    private void ApplyMenuUI()
    {
        mainMenuPanel.SetActive(true);
        gameOverPanel.SetActive(false);

        pauseButton.SetActive(false);
        playButton.SetActive(true);

        countdownText.gameObject.SetActive(false);
    }

    private void ApplyPlayingUI()
    {
        mainMenuPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        pauseButton.SetActive(true);
        playButton.SetActive(false);
    }

    private void ApplyGameOverUI()
    {
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        playButton.SetActive(true);
    }

    private IEnumerator DoCountdown(int seconds, Action onDone)
    {
        if (state == GameState.CountingDown) yield break;

        state = GameState.CountingDown;
        Time.timeScale = 0f;

        mainMenuPanel.SetActive(false);
        countdownText.gameObject.SetActive(true);

        int time = Mathf.Max(0, seconds);
        while (time > 0)
        {
            countdownText.text = time.ToString();
            yield return new WaitForSecondsRealtime(1f);
            time--;
        }

        countdownText.gameObject.SetActive(false);
        onDone?.Invoke();
    }
}
