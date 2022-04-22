using UnityEngine;
using Normal.Realtime;
using UnityEngine.InputSystem;

public class GiantSoundSync : RealtimeComponent<AvatarsSoundsModel>
{
    [SerializeField] AudioSource audiosource;

    [SerializeField] InputActionReference MoveActionButton;


    private void Awake()
    {
        audiosource = GetComponent<AudioSource>();
    }
    private void Start()
    {
        MoveActionButton.action.performed += PlayGiantMoveSound;
        MoveActionButton.action.canceled += StopGiantMoveSound;
    }

    private void PlayGiantMoveSound(InputAction.CallbackContext obj)
    {
        
        model.playGiantMove = true;
        /*if (!audiosource.isPlaying) audiosource.Play();
        Debug.Log("nO");*/

    }

    private void StopGiantMoveSound(InputAction.CallbackContext obj)
    {
        
        model.playGiantMove = false;

        /*if (!audiosource.isPlaying) audiosource.Play();
        Debug.Log("nO");*/

    }

    protected override void OnRealtimeModelReplaced(AvatarsSoundsModel previousModel, AvatarsSoundsModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.playGiantMoveDidChange -= PlaySoundDidChange;
        }
        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                model.playGiantMove= false;
            }
            currentModel.playGiantMoveDidChange += PlaySoundDidChange;
        }
    }

    private void PlaySoundDidChange(AvatarsSoundsModel model, bool value)
    {
        if (value)
        {
            audiosource.Play();
            //Invoke("ResetBool", audiosource.clip.length);
        } else
        {
            audiosource.Stop();
        }
        Debug.Log("modled change");
    }
    private void ResetBool()
    {
        model.playGiantMove= false;
    }
}









