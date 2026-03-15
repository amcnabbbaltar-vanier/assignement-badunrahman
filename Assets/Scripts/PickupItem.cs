using UnityEngine;
using System.Collections;

public class PickupItem : MonoBehaviour
{
    public enum PickupType
    {
        SpeedBoost,
        JumpBoost,
        Score
    }

    [Header("Pickup Settings")]
    public PickupType pickupType;
    public float rotateSpeed = 100f;
    public float hoverAmount = 0.25f;
    public float hoverSpeed = 2f;

    [Header("Effects")]
    public ParticleSystem collectEffect;

    private Vector3 startPosition;
    private Collider pickupCollider;
    private Renderer[] renderers;

    private void Start()
    {
        startPosition = transform.position;
        pickupCollider = GetComponent<Collider>();
        renderers = GetComponentsInChildren<Renderer>();
    }

    private void Update()
    {
        // Hover
        float newY = startPosition.y + Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);

        // Rotate
        transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();

        switch (pickupType)
        {
            case PickupType.SpeedBoost:
                if (playerMovement != null)
                {
                    playerMovement.ActivateSpeedBoost(5f);
                }
                StartCoroutine(RespawnPickup(30f));
                break;

            case PickupType.JumpBoost:
                if (playerMovement != null)
                {
                    playerMovement.ActivateDoubleJumpPowerUp(30f);
                }
                StartCoroutine(RespawnPickup(30f));
                break;

            case PickupType.Score:
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.AddScore(50);
                    //for testing
                    Debug.Log("Score is now: " + GameManager.Instance.score);
                }
                PlayEffect();
                Destroy(gameObject);
                return;
        }

        PlayEffect();
        HidePickup();
    }

    private void HidePickup()
    {
        if (pickupCollider != null)
            pickupCollider.enabled = false;

        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private void ShowPickup()
    {
        if (pickupCollider != null)
            pickupCollider.enabled = true;

        foreach (Renderer r in renderers)
        {
            r.enabled = true;
        }
    }

    private IEnumerator RespawnPickup(float delay)
    {
        yield return new WaitForSeconds(delay);
        ShowPickup();
    }

    private void PlayEffect()
    {
        if (collectEffect != null)
        {
            ParticleSystem effect = Instantiate(collectEffect, transform.position, Quaternion.identity);
            Destroy(effect.gameObject, 2f);
        }
    }
}