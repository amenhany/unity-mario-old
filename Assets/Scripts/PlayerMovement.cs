using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CapsuleCollider2D standingCollider;
    [SerializeField] private CapsuleCollider2D crouchCollider;

    Vector2 smallStandingSize = new Vector2(0.5645186f, 0.9493233f);
    Vector2 smallCrouchingSize = new Vector2(0.5645186f, 0.6581327f);
    Vector2 smallStandingOffset = Vector2.zero;
    Vector2 smallCrouchingOffset = new Vector2(0f, -0.146f);
    Vector2 smallGroundCheck = new Vector2(0f, -0.507f);

    Vector2 bigStandingSize = new Vector2(0.5645186f, 1.359871f);
    Vector2 bigCrouchingSize = new Vector2(0.5645186f, 0.6581327f);
    Vector2 bigStandingOffset = new Vector2(0.025f, 0f);
    Vector2 bigCrouchingOffset = new Vector2(0.025f, -0.291f);
    Vector2 bigGroundCheck = new Vector2(0f, -0.64f);

    float horizontalSpeed;

    [SerializeField] private float normalSpeed = 350f;
    [SerializeField] private float verticalSpeed = 30f;
    [SerializeField] private float crouchSpeed = 0f;
    [SerializeField] private float sprintSpeed = 600f;

    private float coyoteTime = 0.13f;
    private float coyoteTimeCounter = 0f;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter = 0f;

    private bool sprint = false;
    public bool crouch = false;
    public float horizontalMove = 0f;
    private bool facingRight = true;
    public bool lookUp;
    public bool isBig;
    public bool isFire;

    bool isGrounded = false;
    const float groundedRadius = 0.1f;
    float fallMultiplier = 1.2f;
    float lowJumpMultiplier = 6f;

	void Awake() {
        audioManager = FindObjectOfType<AudioManager>();
	}

	void Update() {

        animator.SetFloat("hlSpeed", Mathf.Abs(horizontalMove));
        animator.SetFloat("vlSpeed", rb.velocity.y);
        animator.SetBool("isBig", isBig);
        animator.SetBool("isFire", isFire);

        GetInput();

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, groundLayer);

        //make player look right/left
        if ((horizontalMove > 0 && !facingRight) || (horizontalMove < 0 && facingRight)) {
            facingRight = !facingRight;
            transform.Rotate(0f, 180f, 0f);
		}

        if (isBig) {
            standingCollider.size = bigStandingSize;
            crouchCollider.size = bigCrouchingSize;
            standingCollider.offset = bigStandingOffset;
            crouchCollider.offset = bigCrouchingOffset;
            groundCheck.transform.localPosition = bigGroundCheck;
		} else {
            standingCollider.size = smallStandingSize;
            crouchCollider.size = smallCrouchingSize;
            standingCollider.offset = smallStandingOffset;
            crouchCollider.offset = smallCrouchingOffset;
            groundCheck.transform.localPosition = smallGroundCheck;

        }
    }

	void FixedUpdate() {
        Move();
        Jump();
	}

    void GetInput() {

        horizontalMove = Input.GetAxisRaw("Horizontal");
        if (horizontalMove > -0.1 && horizontalMove < 0.1 && !Input.GetButton("Jump"))
            lookUp = Input.GetButton("Up");
        else lookUp = false;

        sprint = Input.GetButton("Sprint");

        if (Input.GetButton("Crouch")) {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButtonDown("Jump")) {
            jumpBufferCounter = jumpBufferTime;
        } else {
            jumpBufferCounter -= Time.deltaTime;
        }
    }

	void Move() {
        //sprint
        if (sprint && !crouch) {
            horizontalSpeed = sprintSpeed;
            animator.speed = 2f;
        } else {
            horizontalSpeed = normalSpeed;
            animator.speed = 1f;
        }

        //crouch
        if (crouch) {
            horizontalMove *= crouchSpeed;
            crouchCollider.enabled = true;
            standingCollider.enabled = false;
        } else if (!crouch) {
            standingCollider.enabled = true;
            crouchCollider.enabled = false;
        }

        animator.SetBool("isCrouching", crouch);
        animator.SetBool("lookUp", lookUp);

        //horizontal movement
        rb.velocity = new Vector2(horizontalMove * Time.fixedDeltaTime * horizontalSpeed, rb.velocity.y);
    }

    void Jump() {
        //coyote time detection
        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        } else {
            coyoteTimeCounter -= Time.fixedDeltaTime;
        }

        //jump
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, verticalSpeed);
            audioManager.Play("Player Jump");
            jumpBufferCounter = 0f;
        }

        //make fall faster & jump lower if button is pressed for shorter time
        if (rb.velocity.y < 0 && coyoteTimeCounter < 0f) {

            rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.fixedDeltaTime;

        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {

            rb.velocity += Vector2.up * Physics2D.gravity.y * lowJumpMultiplier * Time.fixedDeltaTime;
            coyoteTimeCounter = 0f;
        }
    }
}
