using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using Normal.Realtime;
using TMPro;
using System;

public class AutoQuitScript : MonoBehaviour 
{
    public RealtimeAvatarManagerFork _avatarManager;
    
    public GameObject _playerCount;
    public TextMeshProUGUI countText;
    public int numPlayers;
   
    public ActionBasedSnapTurnProvider snapTurn;
    
    public ActionBasedContinuousMoveProvider conMove;
    [SerializeField] GameObject rayCastHand;
    [SerializeField] GameObject mainHand;





    void Awake()
    {
        _avatarManager.avatarCreated += AvatarCreated;

        //characterController = GetComponent<CharacterController>();

        




    }



    private void AvatarCreated(RealtimeAvatarManagerFork avatarManager, RealtimeAvatarFork avatar, bool isLocalAvatar)
    {

        if (_avatarManager.avatars.Count == 2)
        {
           
            _playerCount.SetActive(false);
            countText.text = "3/3";
            conMove.enabled = true;
            snapTurn.enabled = true;
           
           Debug.Log("count is 3");
        }

      
        else if (_avatarManager.avatars.Count != 2)
        {
            _playerCount.SetActive(true);

            rayCastHand.SetActive(true);
            rayCastHand.transform.position = mainHand.transform.position;
            numPlayers = _avatarManager.avatars.Count;

            countText.text = numPlayers.ToString() + "/3";
        }

      
    }

   















}
