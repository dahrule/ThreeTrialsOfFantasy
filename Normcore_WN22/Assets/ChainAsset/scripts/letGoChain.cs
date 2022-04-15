using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

/// <summary>
/// Forces the release of the  XRinteractable when the player quits the trigger zone defined by a capsule collider.
/// </summary>
public class letGoChain : MonoBehaviour
{
    XRGrabInteractable GrabIn;
    [SerializeField]
    GameObject chain;

    private void Awake()
    {
         GrabIn = chain.GetComponent<XRGrabInteractable>();
       
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("chain"))
        {
            GrabIn.enabled = GrabIn.enabled;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("chain"))
        {
            GrabIn.enabled = !GrabIn.enabled;
        }
        //Debug.Log("letGo!");
    }


}
