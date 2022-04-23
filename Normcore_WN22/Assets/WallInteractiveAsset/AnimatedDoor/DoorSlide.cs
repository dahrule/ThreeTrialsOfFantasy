using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime;

public class DoorSlide : RealtimeComponent<DoorAnimModel>
{

    private Animator animator;
    private AudioSource audioSource;
    [SerializeField] AudioClip congratsSFX;


    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    protected override void OnRealtimeModelReplaced(DoorAnimModel previousModel, DoorAnimModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.openCloseDoorDidChange -= AnimBoolDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current mesh renderer color.
            if (currentModel.isFreshModel)
                currentModel.openCloseDoor = animator.GetBool("DoorOpen");

            // Update the mesh render to match the new model
            UpdateAnimatorBool("DoorOpen");

            // Register for events so we'll know if the color changes later
            currentModel.openCloseDoorDidChange += AnimBoolDidChange;
        }
    }

    private void AnimBoolDidChange(DoorAnimModel model, bool value)
    {
        
        UpdateAnimatorBool("DoorOpen");
        audioSource.Play();
        audioSource.PlayOneShot(congratsSFX);
    }

    private void UpdateAnimatorBool(string parameterName)
    {
        animator.SetBool(parameterName, model.openCloseDoor); 
    }

    public void SetOpenCloseDoor(bool value)
    {
        // Set the bool value on the model
        // This will fire the openCloseDoorDidChange event on the model, which will update the animator controller parameter for both the local player and all remote players.
        model.openCloseDoor=value;
    }
}
