using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundAmbienceManager : MonoBehaviour
{
    [SerializeField] string activator;

    [SerializeField] AudioMixerSnapshot underWater;
    [SerializeField] AudioMixerSnapshot onLand;

    [SerializeField] float transitionTimeInterval=1f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(activator))
        {
            underWater.TransitionTo(transitionTimeInterval);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(activator))
        {
            onLand.TransitionTo(transitionTimeInterval);
        }
    }
}
