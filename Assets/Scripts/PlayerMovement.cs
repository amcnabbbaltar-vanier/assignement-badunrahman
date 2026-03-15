using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private PlayerAnimationController animationController;

    private float originalWalkSpeed;
    private float originalRunSpeed;

    [Header("Movement Settings")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("Ground Jump Settings")]
    public float minJumpForce = 5f;
    public float maxJumpForce = 12f;
    public float maxChargeTime = 3f;

    [Header("Double Jump Settings")]
    public float doubleJumpForce = 8f;
    public bool hasDoubleJumpPowerUp = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    private float moveInput;
    private bool isGrounded;

    private bool isChargingJump = false;
    private float jumpChargeTimer = 0f;

    private bool canUseDoubleJump = false;

    private void Start()
    {
        animationController = GetComponent<PlayerAnimationController>();
        rb = GetComponent<Rigidbody>();
        originalWalkSpeed = walkSpeed;
        originalRunSpeed = runSpeed;
    }

    private void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0)
        {
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (moveInput < 0)
        {
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        // Reset double jump when landing
        if (isGrounded)
        {
            canUseDoubleJump = true;
        }

        // Start charging ground jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isChargingJump = true;
            jumpChargeTimer = 0f;
        }

        // Keep charging while holding
        if (Input.GetKey(KeyCode.Space) && isChargingJump)
        {
            jumpChargeTimer += Time.deltaTime;

            if (jumpChargeTimer >= maxChargeTime)
            {
                PerformGroundJump();
            }
        }

        // Release to jump
        if (Input.GetKeyUp(KeyCode.Space) && isChargingJump)
        {
            PerformGroundJump();
        }

        // Double jump in air
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && hasDoubleJumpPowerUp && canUseDoubleJump)
        {
            PerformDoubleJump();
        }

        // Temporary test key: press J to activate power-up for 30 sec
        if (Input.GetKeyDown(KeyCode.J))
        {
            ActivateDoubleJumpPowerUp(30f);
        }
    }

    private void FixedUpdate()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        rb.linearVelocity = new Vector3(moveInput * currentSpeed, rb.linearVelocity.y, 0f);
    }

    private void PerformGroundJump()
    {
        float chargePercent = jumpChargeTimer / maxChargeTime;
        float finalJumpForce = Mathf.Lerp(minJumpForce, maxJumpForce, chargePercent);

        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * finalJumpForce, ForceMode.Impulse);

        isChargingJump = false;
        jumpChargeTimer = 0f;
    }

    private void PerformDoubleJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(Vector3.up * doubleJumpForce, ForceMode.Impulse);

        canUseDoubleJump = false;

        if (animationController != null)
        {
            animationController.PlayDoubleJumpAnimation();
        }

        Debug.Log("Double Jump Used");
    }

    public void ActivateDoubleJumpPowerUp(float duration)
    {
        StopCoroutine("DoubleJumpPowerUpRoutine");
        StartCoroutine(DoubleJumpPowerUpRoutine(duration));
    }

    private IEnumerator DoubleJumpPowerUpRoutine(float duration)
    {
        hasDoubleJumpPowerUp = true;
        Debug.Log("Double Jump Power-Up Activated");

        yield return new WaitForSeconds(duration);

        hasDoubleJumpPowerUp = false;
        Debug.Log("Double Jump Power-Up Expired");
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public bool IsGrounded()
    {
        return isGrounded;
    }

    public void ActivateSpeedBoost(float duration)
    {
        StopCoroutine("SpeedBoostRoutine");
        StartCoroutine(SpeedBoostRoutine(duration));
    }

    private System.Collections.IEnumerator SpeedBoostRoutine(float duration)
    {
        walkSpeed = originalWalkSpeed * 2f;
        runSpeed = originalRunSpeed * 2f;

        Debug.Log("Speed Boost Activated");

        yield return new WaitForSeconds(duration);

        walkSpeed = originalWalkSpeed;
        runSpeed = originalRunSpeed;

        Debug.Log("Speed Boost Expired");
    }
}
