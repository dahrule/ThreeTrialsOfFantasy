using UnityEngine;
using UnityEngine.InputSystem;

public class JumpTester : MonoBehaviour
{
    [Header("Input Action References")]
    [SerializeField] InputActionReference jumpActionReferenc;
    [SerializeField] InputActionReference moveAction;

    [Header("Moving Parameters")]
    public float speed=3f;
    public float rotationSpeed=1f;
    public float jumpSpeed=5f;

    public float originalStepOffset=1f;
    private Vector2 moveValue;

    private CharacterController characterController;
    private float ySpeed;
    public float newGravity=-2f;
    private float defaultGravity;
    public float activeGravity=-5f;
    public bool isGrounded = true;

    [SerializeField] Camera playerCam;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        jumpActionReferenc.action.performed += startJump;
        moveAction.action.performed += startMove;
    }
   

    // Start is called before the first frame update
    void Start()
    {
        defaultGravity = activeGravity;
        originalStepOffset = characterController.stepOffset;
        characterController.height = 1;
    }

    private void startMove(InputAction.CallbackContext obj)
    {
        moveValue = obj.ReadValue<Vector2>();
    }

    private void startJump(InputAction.CallbackContext obj)
    {
        if (isGrounded)
        {
            isGrounded = false;
            ySpeed = jumpSpeed;
        }
        else
        {
            characterController.stepOffset = 0;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
        if (hit.gameObject.CompareTag("Ground"))
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = 0f;
            isGrounded = true;
            activeGravity = defaultGravity;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 movementDirection = new Vector3(0, 0, moveValue.y);
        movementDirection = Quaternion.Euler(0, playerCam.transform.eulerAngles.y, 0) * movementDirection;

        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * speed;
        movementDirection.Normalize();

        if (!isGrounded)
        {
            ySpeed += activeGravity * Time.deltaTime;

            if (ySpeed < 0f)
            {
                activeGravity = newGravity;
            }
        }
        Vector3 velocity = movementDirection * magnitude;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);
    }
}
