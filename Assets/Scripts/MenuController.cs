using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    public GameObject pausePanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI finalTimeText;

    private bool isPaused = false;

    private void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (finalScoreText != null && GameManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + GameManager.Instance.score;
        }

        if (finalTimeText != null && GameManager.Instance != null)
        {
            finalTimeText.text = "Final Time: " + GameManager.Instance.timer.ToString("F1");
        }
    }

    private void Update()
    {
        if (pausePanel != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PlayGame()
    {
        Debug.Log("Play button clicked");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level11");
    }
    public void QuitGame()
    {
        Debug.Log("Quit button clicked");
        Application.Quit();
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void RestartGameFromEnd()
    {
        Time.timeScale = 1f;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ResetGame();
        }

        SceneManager.LoadScene(1);
    }
}