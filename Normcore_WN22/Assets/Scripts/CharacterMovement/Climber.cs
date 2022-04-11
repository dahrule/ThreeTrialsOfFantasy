using UnityEngine;

/// <summary>
/// //Controls the climbing movement when ClimbInteractables are grabbed. //Uses CharacterController to apply movement.
/// </summary>
/// 
[RequireComponent(typeof(CharacterController))]
public class Climber : MonoBehaviour
{
    [SerializeField] LocomotionSystemsManager locomSysManager;

    private CharacterController characterController;

    private GameObject _climbingHand;
    public GameObject ClimbingHand
    {
        get { return _climbingHand; }
        set { _climbingHand = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_climbingHand)//if a climbing hand has been assigned:
        {
            locomSysManager.ToClimbingSetup();

            Climb();
        }
        else if(_climbingHand==null) //if not climbing
        {
            if (locomSysManager._currentState==LocomotionState.Walk)
            {
                locomSysManager.ClimbingToLandSetup();

            }
            else if (locomSysManager._currentState == LocomotionState.Swim) locomSysManager.ToSwimSetUp();
        }
    }

    void Climb()
    {
        XRDeviceInputTracker devicetracked = _climbingHand.GetComponent<XRDeviceInputTracker>();
        Vector3 velocity = devicetracked.deviceVelocity.action.ReadValue<Vector3>();

        characterController.Move(transform.rotation * -velocity * Time.fixedDeltaTime);
    }
}

