using Normal.Realtime;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Grabbable : MonoBehaviour
{
    private RealtimeTransform realtimeTransform;
    private XRGrabInteractable interactable;

    void Awake()
    {
        realtimeTransform = GetComponent<RealtimeTransform>();
    }

    void Update()
    {
        // Check to see if we think we're holding it, but no longer the owner of it.
        if (interactable.isSelected && !realtimeTransform.isOwnedLocallySelf)
        {
            // Drop it like it's hot!
            interactable.interactionManager.CancelInteractableSelection(interactable);
        }
    }

    protected void OnEnable()
    {
        interactable = GetComponent<XRGrabInteractable>();
        if (interactable != null)
        {
            interactable.selectEntered.AddListener(OnSelectEntered);
        }
    }

    protected void OnDisable()
    {
        if (interactable != null)
        {
            interactable.selectEntered.RemoveListener(OnSelectEntered);
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Once we grab it, we're the owner of it - this will force everyone else to drop it.
        realtimeTransform.RequestOwnership();
    }

}