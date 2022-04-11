using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using System;

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
    
    [SerializeField] GameObject xrRig;
    [SerializeField] GameObject locomotionProvider;
    [SerializeField] GameObject neckReference;

    [SerializeField] AvatarType[] avatars;


    private RealtimeAvatarManager rtAvatarManager;
    private Realtime realtime;


    #region Built-in Functions
    private void Awake()
    {
        //Get references to components
        rtAvatarManager = GetComponent<RealtimeAvatarManager>();
        realtime = GetComponent<Realtime>();

        //subscribe to events
        rtAvatarManager.avatarCreated += AvatarCreated;
    }

    void OnEnable()
    {

    }
    #endregion

    #region Custom Functions
    private void AvatarCreated(RealtimeAvatarManager avatarManager, RealtimeAvatar avatar, bool isLocalAvatar)
    {

        if (realtime.clientID > avatars.Length-1) return;

        //Assign a custom prefab
        avatarManager.localAvatarPrefab = avatars[realtime.clientID].avatarPrefab;

        //Assign a spawnPoint
        xrRig.transform.position= avatars[realtime.clientID].spawnPoint.position;

        //Assign character skills
        RigSetup(avatars[realtime.clientID].avatarType);

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

                locomotionProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                locomotionProvider.GetComponent<MyCharacterControllerDriver>().enabled = true;

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

                locomotionProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
                locomotionProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                locomotionProvider.GetComponent<MyCharacterControllerDriver>().enabled = false;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = true;

                neckReference.SetActive(false);
                break;

            case Type.Giant:
                xrRig.GetComponent<Rigidbody>().isKinematic = true;
                xrRig.GetComponent<CharacterController>().enabled = false;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = false;

                locomotionProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                locomotionProvider.GetComponent<MyCharacterControllerDriver>().enabled =false;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = false;
                break;
        }
    }

    #endregion
}
