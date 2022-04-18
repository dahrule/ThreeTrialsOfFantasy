using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] AudioSource audioS;

    public void Testf()
    {
        audioS.Play();
        Debug.Log("Testing pull chain");
    }
}
