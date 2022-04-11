
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Manages the transition between locomotion states
/// </summary>
/// 
public enum LocomotionState { Swim, Walk };
public class LocomotionSystemsManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] SurfaceChecker surfaceChecker;
    [SerializeField] Swim swim;
    [SerializeField] ActionBasedContinuousMoveProvider continousMove;
    [SerializeField] ActionBasedSnapTurnProvider snapTurn;
    [SerializeField] CharacterController characterController;
    [SerializeField] Collider rbCollider; //the collider required for the Rigidbody.

    public LocomotionState _currentState;

    private void Awake()
    {
        surfaceChecker.OnNeckEntersWater += ChangeState; //Registering for the OnNeckEntersWater on the SurfaceChecker;
        surfaceChecker.OnNeckExitsWater += ChangeState; //Registering for the OnNeckExitsWateron the SurfaceChecker;
    }
    private void Start()
    {
        //set initial state
        _currentState = LocomotionState.Walk;
    }

    void ChangeState(LocomotionState newState)
    {
        _currentState = newState;

        if (_currentState == LocomotionState.Swim) ToSwimSetUp();
    }

    public void ToSwimSetUp()
   {
        snapTurn.enabled = true;
        rbCollider.enabled = true;
        continousMove.enabled = false;
        characterController.enabled = false;
        swim.enabled = true;

    }

    public void ClimbingToLandSetup()
    {
        rbCollider.enabled = false;
        characterController.enabled = true;
        continousMove.enabled = true;
        snapTurn.enabled = true;

        _currentState = LocomotionState.Walk;
    }

    public void ToClimbingSetup()
    {
        continousMove.enabled = false;
        characterController.enabled = true;
        snapTurn.enabled = false;
        swim.enabled = false;
    }

}

    

