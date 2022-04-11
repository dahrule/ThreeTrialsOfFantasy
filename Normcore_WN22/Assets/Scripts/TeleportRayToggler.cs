using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TeleportRayToggler : MonoBehaviour
{
    [SerializeField] GameObject baseControllerObject;
    [SerializeField] GameObject teleportObject;
    [SerializeField] InputActionReference teleportActivationReference;

    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportCancel;


    // Start is called before the first frame update
    void Start()
    {
        teleportActivationReference.action.performed += TeleportModeActivate;
        teleportActivationReference.action.canceled += TeleportModeCancel;
        
    }

    void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        onTeleportActivate.Invoke();
    }

    void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter",0.1f);
    }

    void DeactivateTeleporter()
    {
        onTeleportCancel.Invoke();
    }
}
