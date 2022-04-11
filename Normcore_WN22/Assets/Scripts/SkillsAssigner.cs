using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Utility;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;

public class SkillsAssigner : MonoBehaviour
{

    [SerializeField] XRRig xrRig;
    [SerializeField] GameObject locomotionProvider;
    [SerializeField] RealtimeAvatarFork realTimeAvatar;
    [SerializeField] RealtimeAvatarManagerFork realTimeAvatarManager;

    [SerializeField] Transform[] startingPositions;

    // Start is called before the first frame update
    void Start()
    {
        realTimeAvatarManager.avatarCreated += SetStartingPosition;
        //realTimeAvatarManager.avatarCreated += AssignSkills;
    }


    void SetStartingPosition(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar)
    {
        switch (realTimeAvatarManager._realtime.clientID)
        {
            case 0:
                xrRig.transform.position = startingPositions[0].position;
                break;
            case 1:
                xrRig.transform.position = startingPositions[1].position;
                break;

            case 3:
                xrRig.transform.position = startingPositions[2].position;
                break;
        }
    }
    void AssignSkills(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar)
    {
        switch (realTimeAvatarManager._realtime.clientID)
        {
            case 0:

                break;
            case 1:

                break;

            case 3:

                break;
        }
    }
}
