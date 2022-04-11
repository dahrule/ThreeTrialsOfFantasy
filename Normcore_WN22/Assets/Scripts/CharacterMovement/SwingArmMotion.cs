using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingArmMotion : MonoBehaviour
{
    //Game Objects references
    [SerializeField] GameObject leftHandController;
    [SerializeField] GameObject rightHandController;
    [SerializeField] GameObject forwardDirection;
    [SerializeField] GameObject centerEyeCamera;
    [SerializeField] CharacterController characterController;
    

    //Vector3 positions
    Vector3 leftHand_PreviousFrame_position;
    Vector3 rightHand_PreviousFrame_position;
    Vector3 player_PreviousFrame_position;

    Vector3 leftHand_ThisFrame_position;
    Vector3 rightHand_ThisFrame_position;
    Vector3 player_ThisFrame_position;

    //Movement parameters
    [SerializeField] float speed=70f;
    float handsSpeed;

    #region Built-in functions
    // Start is called before the first frame update
    void Start()
    {
        //Set previous frame positions equal to the game start positions.
        player_PreviousFrame_position = transform.position;
        leftHand_PreviousFrame_position = leftHandController.transform.position;
        rightHand_PreviousFrame_position = rightHandController.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        //Get the forward direction
        float yRotation = centerEyeCamera.transform.eulerAngles.y;
        forwardDirection.transform.eulerAngles = new Vector3(0,yRotation,0);

        //Get current position of hands and player
        player_ThisFrame_position = this.transform.position;
        leftHand_ThisFrame_position = leftHandController.transform.position;
        rightHand_ThisFrame_position = rightHandController.transform.position;

        //Get displacement between previous and current frames.
        var playerDisplacement = Vector3.Distance(player_PreviousFrame_position,player_ThisFrame_position);
        var leftHandDisplacement = Vector3.Distance(leftHand_PreviousFrame_position, leftHand_ThisFrame_position);
        var rightHandDisplacement = Vector3.Distance(rightHand_PreviousFrame_position, rightHand_ThisFrame_position);

        //Add both hands displacements , and substract player's  diplacement to avoid bugs due to the hands being children of the player object.
        handsSpeed = (leftHandDisplacement - playerDisplacement) + (rightHandDisplacement - playerDisplacement);

        //Modify player's position oly after some time has elasped since the level loaded.
        if (Time.timeSinceLevelLoad > 1f)
        { 
            //transform.position += forwardDirection.transform.forward * handsSpeed * speed * Time.deltaTime;
            characterController.Move(forwardDirection.transform.forward * handsSpeed * speed * Time.deltaTime);
        }

        //Set previous positios of hands and player
        leftHand_PreviousFrame_position = leftHand_ThisFrame_position;
        rightHand_PreviousFrame_position = rightHand_ThisFrame_position;
        player_PreviousFrame_position = player_ThisFrame_position;


    }
    #endregion

    #region Custom functions
    #endregion
}
