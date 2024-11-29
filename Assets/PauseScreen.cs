using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    public bool paused = false;

    public void Pause()
    {
        gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
    }
    public void UnPause()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1;
    }
    public void ResumeButton()
    {
        UnPause();
    }
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitButton()
    {
        Application.Quit();
    }
}
