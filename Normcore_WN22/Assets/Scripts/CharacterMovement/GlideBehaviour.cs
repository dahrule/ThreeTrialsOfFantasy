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

    [Header("Sound sfx")]
    [SerializeField] AudioClip jumpsfx;
    [SerializeField] AudioClip landsfx;
    [SerializeField] AudioClip glidesfx;
    [SerializeField] AudioClip fallingsfx;


    private Rigidbody rgbody;
    private XRRig rig;
    private CapsuleCollider capcollider;
    private AudioSource audioSource;
    private float walkingspeed;

    private float fallingTreshold = 2f;


    bool IsGrounded => Physics.Raycast(new Vector3 (transform.position.x,transform.position.y, transform.position.z),Vector3.down,2.0f);
    bool ArmsExtended => Vector3.Dot(RightController.transform.forward, LeftController.transform.forward) < -0.6;
    bool FreeFalling => rgbody.velocity.y > fallingTreshold;

    private void Awake()
    {
        rgbody = GetComponent<Rigidbody>();
        rig = GetComponent<XRRig>();
        capcollider = GetComponent<CapsuleCollider>();
        audioSource = GetComponent<AudioSource>();

        JumpActionActionReference.action.performed += Jump;

        walkingspeed = MoveProvider.moveSpeed; //save original move speed from the Continous Movement Provider component.
    }

    private void OnEnable()
    {
        SetUpRigidbody();
    }

    private void Update()
    {
        UpdateColliderPosition();

    }

    private void FixedUpdate()
    {
        if (!IsGrounded && ArmsExtended) //gliding state
        {
            Glide();
            audioSource.clip = glidesfx;
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else if (!IsGrounded && FreeFalling) //falling state
        {
            audioSource.clip =fallingsfx;
            if(!audioSource.isPlaying) audioSource.Play();

            rgbody.drag = 0;
            MoveProvider.moveSpeed = walkingspeed;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground") && IsGrounded ) //landing state
        {
            audioSource.clip = landsfx;
            audioSource.Play();

            rgbody.drag = 0;
            MoveProvider.moveSpeed = walkingspeed;
        }
   
    }

    private void SetUpRigidbody()
    {
        //Set rigid body's  and collider's states.
        rgbody.constraints = RigidbodyConstraints.FreezeRotation;
        rgbody.useGravity = true;
        rgbody.isKinematic = false;
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

        audioSource.PlayOneShot(jumpsfx);
        rgbody.AddForce(Vector3.up * jumpForce);
    }

    private void Glide()
    {
        rgbody.drag = airDrag; //avatar fallsdown slower when gliding.
        MoveProvider.moveSpeed = glidingSpeed; // avatar moves faster using the joystick while gliding.
    }

   
}
