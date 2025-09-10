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

    [Header("UI Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playButton;

    private int score;

    private void Start()
    {
        ShowMainMenu();
    }

    // Waiting screen
    public void ShowMainMenu()
    {
        Time.timeScale = 1;
        score = 0;
        scoreText.text = "0";

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

        Time.timeScale = 0;
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
            yield return new WaitForSecondsRealtime(1f);
            countdown--;
        }
        yield return new WaitForSecondsRealtime(1f);
        countdownText.gameObject.SetActive(false);
        // Bắt đầu game
        Time.timeScale = 1;
        player.enabled = true;
        EventSystem.current.SetSelectedGameObject(null);
    }

    // Gameover
    public void GameOver()
    {
        Time.timeScale = 0;
        player.enabled = false;

        gameOverPanel.SetActive(true);
        playButton.SetActive(true);
    }


    // Score
    public void Scoring()
    {
        score++;
        scoreText.text = score.ToString();
    }

    // quit
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }
}
