using UnityEngine;

public class RobotController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    //[SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private Vector2 movementInput;
    private bool isFacingRight = true;

    private const string ANIM_BOOL_WALKING = "IsWalking";
    private const string ANIM_FLOAT_MOVE_X = "MoveX";
    private const string ANIM_FLOAT_MOVE_Y = "MoveY";

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        //if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        HandleInput();
        //UpdateAnimation();
        FlipSprite();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        movementInput = new Vector2(horizontal, vertical).normalized;
    }

    private void HandleMovement()
    {
        if (rb != null)
        {
            rb.linearVelocity = movementInput * moveSpeed;
        }
        else
        {
            transform.Translate(movementInput * moveSpeed * Time.fixedDeltaTime);
        }
    }

    /*private void UpdateAnimation()
    {
        if (animator != null)
        {
            bool isMoving = movementInput.magnitude > 0.1f;

            animator.SetBool(ANIM_BOOL_WALKING, isMoving);

            if (isMoving)
            {
                animator.SetFloat(ANIM_FLOAT_MOVE_X, movementInput.x);
                animator.SetFloat(ANIM_FLOAT_MOVE_Y, movementInput.y);
            }
        }
    }*/

    private void FlipSprite()
    {
        if (spriteRenderer != null && movementInput.x != 0)
        {
            if (movementInput.x > 0 && !isFacingRight)
            {
                Flip();
            }
            else if (movementInput.x < 0 && isFacingRight)
            {
                Flip();
            }
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

    public bool IsMoving()
    {
        return movementInput.magnitude > 0.1f;
    }

    public Vector2 GetMovementDirection()
    {
        return movementInput;
    }

    public float GetCurrentSpeed()
    {
        return movementInput.magnitude * moveSpeed;
    }
}