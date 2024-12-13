using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using TMPro;
using UnityEngine.UIElements;
using UnityEditor.VersionControl;

public class GameOver : MonoBehaviour {

    public GameObject scoreUi;

    public bool isOver = false;
    float currTime = 0;
    float bestTime = 0;

    public void OpenDeathScreen()
    {
        isOver = true;
        gameObject.SetActive(true);
        UnityEngine.Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

        currTime = Time.timeSinceLevelLoad;

        var sr = File.ReadAllText("scores.txt");
        bestTime = float.Parse(sr);

        string message = "";
        if (currTime > bestTime) {
            setBestTime(currTime);
            message = $"New record!: {toTime(currTime)}";
        }
        else message = $"Current score: {toTime(currTime)}\r\nBest score: {toTime(bestTime)}";
        scoreUi.GetComponent<TextMeshProUGUI>().text = message;
    }

    string toTime(float time)
    {
        return string.Format("{0:00}:{1:00}", (int)time / 60, (int)time % 60);
    }

    void setBestTime(float time)
    {
        bestTime = time;
        var file = File.CreateText("scores.txt");
        file.WriteLine(bestTime);
        file.Close();
    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ResetScoreButton()
    {
        setBestTime(0);
        scoreUi.GetComponent<TextMeshProUGUI>().text = $"Current score: {toTime(currTime)}";
    }

    public void ExitButton()
    {
        Application.Quit();
    }

}
