using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Game Data")]
    public int score = 0;
    public int health = 3;
    public float timer = 0f;

    private void Awake()
    {
        // Singleton logic
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Timer: " + timer);
            Debug.Log("Score: " + score);
            Debug.Log("Health: " + health);
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void LoseHealth(int amount)
    {
        health -= amount;

        if (health <= 0)
        {
            health = 3;
            RestartCurrentLevel();
        }
    }

    public void RestartCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetGame()
    {
        score = 0;
        health = 3;
        timer = 0f;
    }
}