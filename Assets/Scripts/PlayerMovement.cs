using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // vibe coded

    [Header("Movement")]
    public float walkSpeed = 3f;
    public float sprintSpeed = 6f;
    public bool isSprinting = false;
    public bool canSprint = true; // is modified by PlayerStats
    public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Acceleration / Deceleration")]
    public float acceleration = 10f;
    public float deceleration = 10f;
    private Vector3 currentHorizontalVelocity;

    [Header("Jump / Gravity")]
    public float gravity = -9.81f;
    [Tooltip("Peak jump height in meters")]
    public float jumpHeight = 1.5f;

    [Header("Head bob (camera)")]
    public Transform playerCamera;
    public float bobSpeed = 10f;
    public float bobAmountVertical = 0.05f;
    public float bobAmountHorizontal = 0.03f;
    [Range(1f, 20f)] public float bobSmooth = 8f;
    public float sprintBobMultiplier = 1.6f;

    // Internal
    private CharacterController characterController;
    private Vector3 velocity; // used for vertical velocity (gravity + jump)
    private Vector3 moveInput; // x/z input
    private Vector3 cameraOriginalLocalPos;
    private float bobTimer = 0f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        if (GameManager.Instance.isPaused)
            return;
        HandleMovement();
        HandleHeadBob();
    }

    private void HandleMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        moveInput = transform.right * moveX + transform.forward * moveZ;

        isSprinting = Input.GetKey(sprintKey) && moveInput.sqrMagnitude > 0.01f && canSprint;
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        Vector3 move = moveInput.normalized * currentSpeed;

        float rate = (move.magnitude > 0.02f) ? acceleration : deceleration;

        // Smoothly move current velocity toward target
        currentHorizontalVelocity = Vector3.MoveTowards(
            currentHorizontalVelocity,
            move,
            rate * Time.deltaTime
        );

        // Apply gravity
        if (characterController.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f; // Small negative to keep grounded

            if (Input.GetButtonDown("Jump"))
            {
                // v = sqrt(2 * g * h) but gravity is negative, so use -2 * gravity
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;

        Vector3 finalMove = currentHorizontalVelocity + new Vector3(0, velocity.y, 0);
        characterController.Move(finalMove * Time.deltaTime);
    }

    void HandleHeadBob()
    {
        if (playerCamera == null) return;

        // Only bob while grounded and moving
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).magnitude;
        bool moving = characterController.isGrounded && inputMagnitude > 0.01f;

        // increase bob rate based on movement and sprinting
        bool isSprinting = Input.GetKey(sprintKey) && inputMagnitude > 0.01f;
        float targetBobSpeed = bobSpeed * (isSprinting ? sprintBobMultiplier : 1f);

        if (moving)
        {
            // increment bob timer according to movement intensity
            bobTimer += Time.deltaTime * targetBobSpeed * Mathf.Clamp01(inputMagnitude);

            // horizontal sway (x) and vertical bounce (y)
            float bobX = Mathf.Sin(bobTimer) * bobAmountHorizontal * inputMagnitude;
            float bobY = Mathf.Abs(Mathf.Cos(bobTimer)) * bobAmountVertical * inputMagnitude;

            Vector3 targetPos = cameraOriginalLocalPos + new Vector3(bobX, bobY, 0f);

            // smooth towards target
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, targetPos, bobSmooth * Time.deltaTime);
        }
        else
        {
            // return camera to original position smoothly
            bobTimer = 0f; // optional: reset to keep bob synced when starting to move again
            playerCamera.localPosition = Vector3.Lerp(playerCamera.localPosition, cameraOriginalLocalPos, bobSmooth * Time.deltaTime);
        }
    }
}
