using TMPro;
using UnityEngine;

public class HUDController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI timerText;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        scoreText.text = "Score: " + GameManager.Instance.score;
        healthText.text = "Health: " + GameManager.Instance.health;
        timerText.text = "Time: " + GameManager.Instance.timer.ToString("F1");
    }
}