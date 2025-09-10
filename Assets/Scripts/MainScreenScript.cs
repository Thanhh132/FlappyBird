using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainScreenScript : MonoBehaviour
{
    public Text countdownText;
    public GameObject getReadyImage;
    public string playSceneName = "GamePlayScreen";
    private bool isCounting = false;
    public void Update()
    {
        if (!isCounting && Input.GetMouseButtonDown(0))
        {
            getReadyImage.SetActive(false);
            StartCoroutine(CountdownAndLoad());
        }
    }

    private IEnumerator CountdownAndLoad()
    {
         isCounting = true;
        int countdown = 3;

        while (countdown > 0)
        {
            countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1f);
            countdown--;
        }
        countdownText.text = "";
        SceneManager.LoadScene("GamePlayScreen");
    }
}
