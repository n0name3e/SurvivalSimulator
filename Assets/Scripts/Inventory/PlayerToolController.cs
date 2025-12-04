using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerToolController : MonoBehaviour
{
    public static PlayerToolController Instance { get; private set; }
    private PlayerMovement playerMovement;

    public GameObject toolSlotUI;

    public Transform handTransform;
    public GameObject toolObject;
    public Tool equippedTool;

    public float bobAmount = 0.02f;
    public float sprintBobMult = 2f;
    public float bobSmooth = 10f;

    public float swayAmount = 2f;
    public float maxSwayAmount = 5f;
    public float swaySmooth = 8f;

    public float fallSwayAmount = 0.005f; // How much the weapon moves when falling
    public float maxFallSway = 0.1f;      // Clamp to prevent it flying off screen

    private Vector3 initialHandPosition;
    private Quaternion initialHandRotation;
    private float bobTimer;


    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;
    }
    private void Start()
    {
        if (handTransform != null)
        {
            initialHandPosition = handTransform.localPosition;
            initialHandRotation = handTransform.localRotation;
        }

        playerMovement = FindObjectOfType<PlayerMovement>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanSwing())
        {
            SwingWithTool();
        }
        ApplyBob();
        ApplySway();
    }
    public bool CanSwing()
    {
        if (equippedTool == null) return false;
        if (GameManager.Instance.isPaused) return false;
        if (EventSystem.current.IsPointerOverGameObject()) return false;


        return true;
    }
    public void EquipTool(Tool tool)
    {
        if (equippedTool != null)
        {
            Inventory.Instance.AddItem(equippedTool, 1);
            Destroy(toolObject);
        }
        toolObject = Instantiate(tool.toolPrefab, handTransform);

        equippedTool = tool;
    }
    public void SwingWithTool()
    {
        toolObject.GetComponentInChildren<Animator>().Play("Swing");
    }

    // called in animation in certain frame
    public void HitWithTool()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, 3f))
        {
            if (hit.collider.TryGetComponent<WorldRecource>(out WorldRecource resource))
            {
                resource.TakeDamage(equippedTool.damage);
            }
            if (hit.collider.TryGetComponent<EnemyHealth>(out EnemyHealth enemyHealth))
            {
                enemyHealth.TakeDamage(equippedTool.damage);
            }
        }
        print("hit with tool");
    }
    #region Bob and Sway
    private void ApplyBob()
    {
        if (toolObject == null)
            return;
        float inputMagnitude = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")).magnitude;
        Vector3 targetPosition = initialHandPosition;
        if (inputMagnitude > 0.01f)
        {
            float bobSpeed = playerMovement.isSprinting ? playerMovement.bobSpeed * sprintBobMult : playerMovement.bobSpeed;
            bobTimer += Time.deltaTime * bobSpeed;
            float horizontalBob = Mathf.Sin(bobTimer) * bobAmount;
            float verticalBob = Mathf.Cos(bobTimer * 2) * bobAmount;
            targetPosition = initialHandPosition + new Vector3(horizontalBob, verticalBob, 0f);
            //handTransform.localPosition = Vector3.Lerp(handTransform.localPosition, targetPosition, Time.deltaTime * bobSmooth);
        }
        else
        {
            targetPosition = initialHandPosition;
            bobTimer = 0f;
            //handTransform.localPosition = Vector3.Lerp(handTransform.localPosition, initialHandPosition, Time.deltaTime * bobSmooth);
        }
        float verticalVelocity = playerMovement.velocity.y;

        // Calculate drag:
        // If falling (negative velocity), weapon drags UP (positive offset).
        // If jumping (positive velocity), weapon drags DOWN (negative offset).
        float fallOffsetY = -verticalVelocity * fallSwayAmount;

        // Clamp it so the weapon doesn't disappear into the ceiling/floor during long falls
        fallOffsetY = Mathf.Clamp(fallOffsetY, -maxFallSway, maxFallSway);

        // Apply to target
        targetPosition.y += fallOffsetY;

        // Smoothly move towards target (Bob + Fall Sway combined)
        handTransform.localPosition = Vector3.Lerp(handTransform.localPosition, targetPosition, Time.deltaTime * bobSmooth);
    }
    private void ApplySway()
    {
        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X") * swayAmount;
        float mouseY = Input.GetAxis("Mouse Y") * swayAmount;

        // Clamp the values so the weapon doesn't spin around 360 degrees
        mouseX = Mathf.Clamp(mouseX, -maxSwayAmount, maxSwayAmount);
        mouseY = Mathf.Clamp(mouseY, -maxSwayAmount, maxSwayAmount);

        // Calculate target rotation (opposite to mouse movement to create "lag")
        Quaternion swayX = Quaternion.AngleAxis(-mouseY, Vector3.right);
        Quaternion swayY = Quaternion.AngleAxis(mouseX, Vector3.up);
        Quaternion targetRotation = initialHandRotation * swayX * swayY;

        // Smoothly rotate towards target
        handTransform.localRotation = Quaternion.Lerp(handTransform.localRotation, targetRotation, Time.deltaTime * swaySmooth);
    }
    #endregion
}
