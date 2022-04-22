using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Swim mechanic using player hands and physics.
/// </summary>
///

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Swim : MonoBehaviour
{
    [Header ("Swim physics parameters")]
    [SerializeField] float swimForce=3f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce = 3f; //minimum stroke force required to move.
    [SerializeField] float minTimeBetweenStrokes=0f; //minimum time required between strokes to validate movement.

    [Header("Swim Input controllers")]
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] InputActionReference leftControllerVelocity;

    [Header("Tracking References")]
    [SerializeField] Transform trackingReference;
    [SerializeField] Transform neckReference;

    public Transform waterSurface;
    [SerializeField] Transform XRcamera;
    [SerializeField] string tagForWater;
    [SerializeField] AudioClip enterWatersfx;


    Rigidbody rgbody;
    CapsuleCollider capsuleCollider;
    AudioSource audioSource;
    float coolDownTimer;
   

    // Start is called before the first frame update
    void Awake()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        rgbody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        //Set rigid body's  and collider's states.
        rgbody.useGravity = false;
        rgbody.constraints = RigidbodyConstraints.FreezeRotation;
        AddDrag();

        capsuleCollider.enabled = false;
        capsuleCollider.height = 0.5f;
        capsuleCollider.radius = 0.25f;
         
    }

    private void OnEnable()
    {
        capsuleCollider.enabled = true;
        
    }

    private void OnDisable()
    {
        capsuleCollider.enabled = false;
    }

    void Update()
    {
        //Update collider position 
        capsuleCollider.center = XRcamera.localPosition;

        ClampVerticalMovement();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagForWater))
        {
            audioSource.PlayOneShot(enterWatersfx);
        }
    }

    private void ClampVerticalMovement()
    {
        //Restricts the vertical movement of the swimmer no further than the water surface.
        float lowerLimit = -100f;
        float upperLimit = waterSurface.position.y - (neckReference.position.y - transform.position.y) - 0.1f;

        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, lowerLimit, upperLimit);

        transform.position = clampedPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        coolDownTimer += Time.fixedDeltaTime;
        if (coolDownTimer > minTimeBetweenStrokes && HandsInWater())
        {

            //Calculate stroke strength
            var leftHandVelocity = leftControllerVelocity.action.ReadValue<Vector3>();
            var rightHandVelocity = rightControllerVelocity.action.ReadValue<Vector3>();

            Vector3 localVelocity = (leftHandVelocity + rightHandVelocity) * -1;


            //Apply swim motion if stroke is strong enough
            if (localVelocity.sqrMagnitude > minForce * minForce)
            {
                Vector3 worldVelocity = trackingReference.TransformDirection(localVelocity);
                rgbody.AddForce(worldVelocity * swimForce, ForceMode.Acceleration);

                coolDownTimer = 0f;
            }

        }

    }

    private bool HandsInWater()
    {
        return true;
    }

    private void AddDrag()
   {

        rgbody.drag = dragForce;

        //If drag is set manually, use this in update.
        /*if (rgbody.velocity.sqrMagnitude > 0.01f)
        {
            rgbody.AddForce(-rgbody.velocity * dragForce, ForceMode.Acceleration);
        }*/
    }
}
