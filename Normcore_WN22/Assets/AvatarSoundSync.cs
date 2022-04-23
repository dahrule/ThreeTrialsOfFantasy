
using UnityEngine;

using Normal.Realtime;
using UnityEngine.InputSystem;


[RequireComponent(typeof(AudioSource))]
public class AvatarSoundSync : RealtimeComponent<AvatarSoundModel>
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] InputActionReference JumpActionButton;


    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        JumpActionButton.action.performed += PlayJumpSound;
    }

    private void PlayJumpSound(InputAction.CallbackContext obj)
    {

        model.playSoud = true;
        

    }

    protected override void OnRealtimeModelReplaced(AvatarSoundModel previousModel, AvatarSoundModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.playSoudDidChange -= PlaySoundDidChange;
        }
        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                model.playSoud = false;
            }
            currentModel.playSoudDidChange += PlaySoundDidChange;
        }
    }

    private void PlaySoundDidChange(AvatarSoundModel model, bool value)
    {
        if (value)
        {
            audioSource.Play();
            Invoke("ResetBool", audioSource.clip.length);
        }
    }
    private void ResetBool()
    {
        model.playSoud = false;
    }

    public void SetPlaySound(bool value)
    {
        model.playSoud = value;
    }

}
