using UnityEngine;

public class TrapHazard : MonoBehaviour
{
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;

        if (!other.CompareTag("Player"))
            return;

        triggered = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.LoseHealth(1);
            GameManager.Instance.RestartCurrentLevel();
        }
    }
}