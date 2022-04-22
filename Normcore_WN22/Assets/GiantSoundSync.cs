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
    }

    private void PlayGiantMoveSound(InputAction.CallbackContext obj)
    {
        Debug.Log(obj.ReadValue<Vector3>());
        //model.playGiantMove = true;

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
        if (!audiosource.isPlaying)
        {
            audiosource.Play();
            Invoke("ResetBool", audiosource.clip.length);
        }
        Debug.Log("modled change");
    }
    private void ResetBool()
    {
        model.playGiantMove= false;
    }
}









