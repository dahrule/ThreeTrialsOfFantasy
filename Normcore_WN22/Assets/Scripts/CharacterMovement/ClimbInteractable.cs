using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

//Attach this script to objects that will act as grabbing points during climbing.
//The only purpose of this script is to assign a climbing hand into the Climber script when a ClimbInteractable is grabbed.
public class ClimbInteractable : XRBaseInteractable
{
    [SerializeField] Climber climber;

    XRBaseInteractor _interactor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        _interactor= args.interactor;
        

        if (_interactor is XRDirectInteractor)//Ensures that the object grabbed is of type XRDirectInteractor and not XRRayInteractor.
        {
            climber.ClimbingHand=_interactor.gameObject; 
        }
        
        //Debug.Log("ENTERED");
    }


    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
       _interactor = args.interactor;

        if (_interactor is XRDirectInteractor) //Ensures that the object grabbed is of type XRDirectInteractor and not XRRayInteractor.
        { 
            if (climber.ClimbingHand && climber.ClimbingHand.name == _interactor.name) //Clear the current hand if the object sending the OnSelectedExited event is the same that was grabbing a climbing point.
            {
                climber.ClimbingHand = null;
            }
        }

        //Debug.Log("EXITED");
    }
}
