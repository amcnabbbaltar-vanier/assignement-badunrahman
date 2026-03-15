using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody rb;
    private PlayerMovement playerMovement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        float horizontalSpeed = Mathf.Abs(rb.linearVelocity.x);
        animator.SetFloat("Speed", horizontalSpeed);

        animator.SetBool("IsGrounded", playerMovement.IsGrounded());
    }

    public void PlayDoubleJumpAnimation()
    {
        animator.SetTrigger("DoubleJump");
    }
}