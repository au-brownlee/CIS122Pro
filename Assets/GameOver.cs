using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Setup()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
