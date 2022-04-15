using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;


/// <summary>
/// Controls the jumping and gliding using physics
/// </summary>

[RequireComponent(typeof(Rigidbody))]
public class GlideBehaviour : MonoBehaviour
{
    [Header("References")]
    [SerializeField] InputActionReference JumpActionActionReference;
    [SerializeField] GameObject RightController;
    [SerializeField] GameObject LeftController;
    [SerializeField] ActionBasedContinuousMoveProvider MoveProvider;

    [Header("Locomotion parameters")]
    [SerializeField] float jumpForce=500f;
    [Tooltip("defines the falling speed")] [SerializeField] float airDrag=5f;
    [SerializeField] float glidingSpeed = 4f;
    

    private Rigidbody rgbody;
    private XRRig rig;
    private CapsuleCollider capcollider;
    private float walkingspeed;
    
 
    bool IsGrounded => Physics.Raycast(new Vector3 (transform.position.x,transform.position.y, transform.position.z),Vector3.down,2.0f);
    bool armsExtended => Vector3.Dot(RightController.transform.forward, LeftController.transform.forward) < -0.6;


    private void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
        rig = GetComponent<XRRig>();
        capcollider = GetComponent<CapsuleCollider>();

        JumpActionActionReference.action.performed += Jump;

        walkingspeed = MoveProvider.moveSpeed; //save original move speed from the Continous Movement Provider component.
    }

    private void OnEnable() 
    {
        //Set rigid body's  and collider's states.
        rgbody.constraints = RigidbodyConstraints.FreezeRotation;
        rgbody.useGravity = true;
    }
    private void Update()
    {
        UpdateColliderPosition();

    }

    private void FixedUpdate()
    {
        if (!IsGrounded && armsExtended)
        {
            Glide();
        }
        else
        {
            rgbody.drag = 0;
            MoveProvider.moveSpeed = walkingspeed;
        }
    }

    private void UpdateColliderPosition()
    {
        var center = rig.cameraInRigSpacePos;
        capcollider.center = new Vector3(center.x, capcollider.center.y, center.z);
        capcollider.height = rig.cameraInRigSpaceHeight;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        if (!IsGrounded) return;

        rgbody.AddForce(Vector3.up * jumpForce);
    }

    private void Glide()
    {
        rgbody.drag = airDrag; //avatar fallsdown slower when gliding.
        MoveProvider.moveSpeed = glidingSpeed; // avatar moves faster using the joystick while gliding.
    }

}
