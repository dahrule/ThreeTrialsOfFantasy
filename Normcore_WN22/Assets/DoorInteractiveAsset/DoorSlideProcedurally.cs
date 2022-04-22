using System.Collections;
using UnityEngine;
using Normal.Realtime;

/// <summary>
/// Controls door sliding procedurally.
/// </summary>

[RequireComponent(typeof(AudioSource))]
public class DoorSlideProcedurally : RealtimeComponent<DoorModel>
{

    [SerializeField] Transform door;
    [SerializeField] float slidingSpeed = 1f;

    private IEnumerator doorMovement;
    private AudioSource audioSource;
    private bool _isDoorSliding;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void SetTarget(Transform target)
    {
        if(doorMovement!=null) StopCoroutine(doorMovement);

        doorMovement = DoorMovementRoutine(target.position);
        StartCoroutine(doorMovement);
    }

 
    IEnumerator DoorMovementRoutine(Vector3 target)
    {
        while(Vector3.Distance(door.position,target)>0.05f)
        {
            SetIsDoorSliding(true);

            //slide door towards target
            door.position = Vector3.Lerp(door.position,target, slidingSpeed*Time.deltaTime);

            //Play soundsfx as long as door hasnot reached the target.
            if (_isDoorSliding && !audioSource.isPlaying) audioSource.Play(); 

            yield return null;
        }

        //Stop playing soundsfx when door reaches the target.
        SetIsDoorSliding(false);
        if(!_isDoorSliding) audioSource.Stop(); 
           
    }

    protected override void OnRealtimeModelReplaced(DoorModel previousModel, DoorModel currentModel)
    {
        if (previousModel != null)
        {
            // Unregister from events
            previousModel.isDoorSlidingDidChange -=DoorSlidingDidChange;
        }

        if (currentModel != null)
        {
            // If this is a model that has no data set on it, populate it with the current value of isDoorSliding variable.
            if (currentModel.isFreshModel)
                currentModel.isDoorSliding = _isDoorSliding;

            // Update the local isDoorSliding variable to match the new model
            UpdateIsDoorSliding();

            // Register for events so we'll know when the model changes later
            currentModel.isDoorSlidingDidChange += DoorSlidingDidChange;
        }
    }

    private void DoorSlidingDidChange(DoorModel model, bool value)
    {

        UpdateIsDoorSliding();
        
    }

    private void UpdateIsDoorSliding()
    {
        _isDoorSliding = model.isDoorSliding;
    }

    public void SetIsDoorSliding(bool value)
    {
        model.isDoorSliding = value;
    }

}
