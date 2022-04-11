using System;
using UnityEngine;

/// <summary>
/// /Assign this script to the collider object that will check when the water level is at the avatar's neck level.
/// </summary>
/// 
[RequireComponent(typeof(MyRotationConstraint))]
[RequireComponent(typeof(BoxCollider))]
public class SurfaceChecker : MonoBehaviour
{
    [SerializeField] string tagForWater;
    public event Action<LocomotionState>OnNeckEntersWater;
    public event Action <LocomotionState>OnNeckExitsWater;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(tagForWater))
        { 
           OnNeckEntersWater?.Invoke(LocomotionState.Swim); 
        }
        Debug.Log("EnterWater");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(tagForWater))
        {
            OnNeckExitsWater?.Invoke(LocomotionState.Walk);
            Debug.Log("ExitWater");
        }
    }
}
