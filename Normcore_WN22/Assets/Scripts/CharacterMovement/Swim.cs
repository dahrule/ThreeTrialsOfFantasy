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
    [SerializeField] float swimForce=2f;
    [SerializeField] float dragForce = 1f;
    [SerializeField] float minForce = 1f; //minimum stroke force required to move.
    [SerializeField] float minTimeBetweenStrokes;

    [Header("Swim Input controllers")]
    [SerializeField] InputActionReference rightControllerVelocity;
    [SerializeField] InputActionReference leftControllerVelocity;

    [Header("Tracking References")]
    [SerializeField] Transform trackingReference;
    [SerializeField] Transform neckReference;

    [SerializeField] Transform waterSurface;
    [SerializeField] Transform XRcamera;


    Rigidbody rgbody;
    CapsuleCollider capsuleCollider;
    float coolDownTimer;
   

    // Start is called before the first frame update
    void Awake()
    {
        //Set rigid body's  and collider initial states
        capsuleCollider = GetComponent<CapsuleCollider>();
        rgbody = GetComponent<Rigidbody>();
        rgbody.useGravity = false;
        capsuleCollider.enabled = false;
        capsuleCollider.height = 0.5f;
        capsuleCollider.radius = 0.25f;
        
        rgbody.constraints = RigidbodyConstraints.FreezeRotation;
        rgbody.drag = dragForce;
        
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
        capsuleCollider.center=XRcamera.localPosition;

        //Clamp the vertical position of the swimmer so he cannot move above surface.
        Vector3 clampedPosition = transform.position;
        //clampedPosition.y= Mathf.Clamp(clampedPosition.y, -100,waterSurface.position.y+neckReference.position.y+neckOffset);
        clampedPosition.y= Mathf.Clamp(clampedPosition.y, -100, waterSurface.position.y-(neckReference.position.y- transform.position.y)-0.1f);

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

    /*private void AddDrag()
   {
       *//*if (rgbody.velocity.sqrMagnitude > 0.01f)
       {
           rgbody.AddForce(-rgbody.velocity * dragForce, ForceMode.Acceleration);
       }*//*
   }*/
}
