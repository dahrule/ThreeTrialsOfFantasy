using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;
using Normal.Realtime;

[RequireComponent(typeof(AudioSource))]
public class AvatarSoundSync : RealtimeComponent<AvatarSoundModel>
{
    [SerializeField] AudioSource audioSource;
    

    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
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
