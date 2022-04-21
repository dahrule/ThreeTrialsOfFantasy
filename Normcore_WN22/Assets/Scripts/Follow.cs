using UnityEngine;

/// <summary>
/// SCRIPT TAKEN FROM VALEM: https://www.youtube.com/watch?v=tBYl-aSxUe0
/// Maps Inverse Kinematics bones to XRRig  parts (head, hands).
/// </summary>
[System.Serializable]
public class FollowOffset
{
    public Transform XRRigTarget;
    public Transform IKTarget;
    public Vector3 position;
    public Vector3 rotation;


    public void Map()
    {
        IKTarget.position = XRRigTarget.TransformPoint(position);
        IKTarget.rotation = XRRigTarget.rotation * Quaternion.Euler(rotation);
    }
}

public class Follow : MonoBehaviour
{
    [SerializeField] FollowOffset head;
    [SerializeField] FollowOffset leftHand;
    [SerializeField] FollowOffset rightHand;

    [SerializeField] Transform IKHeadConstraint;
    [SerializeField] Vector3 headBodyOffset;

    [SerializeField] float turnSmoothness = 3f;

    private void Start()
    {
       headBodyOffset = transform.position - IKHeadConstraint.position;
    }

    void LateUpdate()
    {
        transform.position = IKHeadConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(IKHeadConstraint.up, Vector3.up).normalized,Time.deltaTime*turnSmoothness);
        //transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized; //body moves instantly with head


        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
