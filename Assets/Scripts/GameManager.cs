using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerScript player;
    public Text scoreText;
    [SerializeField]private int score;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject playButton;
    // Start is called before the first frame update
    void Start()
    {
        PauseGame();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Scoring()
    {
        Debug.Log("+1 point");
        score++;
        scoreText.text = score.ToString();
    }

    public void GameOver()
    {
        PauseGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
        playButton.SetActive(true);
        gameOverPanel.SetActive(true);
        player.enabled = false;
    }

    public void PlayGame()
    {
        Time.timeScale = 1;
        score = 0;
        scoreText.text = score.ToString();

        playButton.SetActive(false);
        gameOverPanel.SetActive(false);

        PipeMovement[] pipes = FindObjectsOfType<PipeMovement>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        player.enabled = true;
        
        EventSystem.current.SetSelectedGameObject(null);
    }
}
