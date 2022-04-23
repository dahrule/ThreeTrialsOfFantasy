using UnityEngine;
using Normal.Realtime;
using UnityEngine.XR.Interaction.Toolkit;
using System;

/// <summary>
/// Assigns each player a unique avatar with different skills, names, and starting positions.
/// </summary>
public enum Type { Amphibian, Sprite, Giant };

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

    private RealtimeAvatarManagerFork _rtAvatarManager;
    private Realtime _realtime;

    private static string[] _adjectives = new string[] { "Magical", "Cool", "Nice", "Funny", "Fancy", "Glorious", "Weird", "Awesome" }; //adjetives for composing the player name


    #region Built-in Functions
    private void Awake()
    {
        //Get references to components
        _rtAvatarManager = GetComponent<RealtimeAvatarManagerFork>();
        _realtime = GetComponent<Realtime>();

        //subscribe to events
        _rtAvatarManager.avatarCreated += AssignAvatarName;
        _realtime.didConnectToRoom += AssignAvatarCharacteristics;
    }

    #endregion

    #region Custom Functions
    private void AssignAvatarCharacteristics(Realtime realtime)
    {
        if (realtime.clientID > avatarTypes.Length - 1) return;

        //Assign a custom avatar prefab
        _rtAvatarManager.localAvatarPrefab = avatarTypes[realtime.clientID].avatarPrefab;

        //Assign a spawnPoint
        xrRig.transform.position = avatarTypes[realtime.clientID].spawnPoint.position;

        //Assign avatar skills through rig setup
        AvatarRigSetup(avatarTypes[realtime.clientID].avatarType);
    }

    private void AssignAvatarName(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar)
    {
        if (avatar.isOwnedLocallyInHierarchy)
        {
            if (_realtime.clientID > avatarTypes.Length - 1) return;

            //Get character type
            string characterType = avatarTypes[_realtime.clientID].avatarType.ToString();

            // Generate a funny random name
            string composedPlayername= _adjectives[UnityEngine.Random.Range(0, _adjectives.Length)] + " " + characterType;

            avatar.gameObject.GetComponent<PlayerInfo>().SetPlayerName(composedPlayername);
        }
 
    }


    void AvatarRigSetup(Type avatarType)
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
                xrRig.GetComponent<GlideBehaviour>().enabled = false;

                neckReference.SetActive(true);
                break;

            case Type.Sprite:
                /*xrRig.GetComponent<Rigidbody>().isKinematic = true;
                xrRig.GetComponent<CharacterController>().enabled =true;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = false;

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = false;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = false;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = false;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<JumpTester>().enabled = true;

                neckReference.SetActive(false);*/

                xrRig.GetComponent<CharacterController>().enabled = false;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = false;

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = false;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = true;
                xrRig.GetComponent<GlideBehaviour>().enabled = true;

                neckReference.SetActive(false);
                break;

            case Type.Giant:
                xrRig.GetComponent<Rigidbody>().isKinematic = false;
                xrRig.GetComponent<CharacterController>().enabled = true;
                xrRig.GetComponent<Climber>().enabled = false;
                xrRig.GetComponent<LocomotionSystemsManager>().enabled = true;

                xrRig.transform.localScale=new Vector3(2f,2f,2f);

                locomotionSystemProvider.GetComponent<ActionBasedSnapTurnProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<ActionBasedContinuousMoveProvider>().enabled = true;
                locomotionSystemProvider.GetComponent<MyCharacterControllerDriver>().enabled = true;

                xrRig.GetComponent<Swim>().enabled = false;
                xrRig.GetComponent<CapsuleCollider>().enabled = false;
                xrRig.GetComponent<GlideBehaviour>().enabled = false;

                neckReference.SetActive(false);
                break;
        }
    }

    #endregion
}
