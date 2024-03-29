using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using System;

/// <summary>
/// Manages the starting position, the local avatar prefab, and the scripts on the local XRrig that provide characters different movement skills.
/// </summary>
public enum Type { Amphibian, Sprit, Giant };

[Serializable]
public struct AvatarType
{
    public Type avatarType;
    public GameObject avatarPrefab;
    public Transform spawnPoint;
    //public GameObject skills;
}

public class MyAvatarManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] GameObject xrRig;
    [SerializeField] GameObject locomotionSystemProvider;
    [SerializeField] GameObject neckReference;

    [SerializeField] AvatarType[] avatarTypes;


    private RealtimeAvatarManagerFork rtAvatarManager;
    private Realtime realtime;


    #region Built-in Functions
    private void Awake()
    {
        //Get references to components
        rtAvatarManager = GetComponent<RealtimeAvatarManagerFork>();
        realtime = GetComponent<Realtime>();

        //subscribe to events
        rtAvatarManager.avatarCreated += AvatarCreated;
       
    }

    #endregion

    #region Custom Functions
    private void AvatarCreated(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar)
    {

        if (realtime.clientID > avatarTypes.Length-1) return;

        //Assign a custom prefab
        if(isLocalAvatar) rtAvatarManager.localAvatarPrefab = avatarTypes[realtime.clientID].avatarPrefab;
       

        //Assign a spawnPoint
        xrRig.transform.position= avatarTypes[realtime.clientID].spawnPoint.position;

        //Assign character skills
        RigSetup(avatarTypes[realtime.clientID].avatarType);

    }


    void RigSetup(Type avatarType)
    {
        switch (avatarType)
        {
            case Type.Amphibian:
                xrRig.GetComponent<Rigidbody>().isKinematic = false;
                xrRig.GetComponent<CharacterController>().enabled = true;
                xrRig.GetComponent<Climber>().enabled= true;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = true;

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = true;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = false;

                neckReference.SetActive(true);
                break;

            case Type.Sprit:
                xrRig.GetComponent<Rigidbody>().isKinematic = true;
                xrRig.GetComponent<CharacterController>().enabled =true;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = false;

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = false;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = true;

                neckReference.SetActive(false);
                break;

            case Type.Giant:
                xrRig.GetComponent<Rigidbody>().isKinematic = false;
                xrRig.GetComponent<CharacterController>().enabled = true;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = true;

                xrRig.transform.localScale=new Vector3(3f,3f,3f);

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = true;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = false;

                neckReference.SetActive(false);
                break;
        }
    }

    #endregion
}
