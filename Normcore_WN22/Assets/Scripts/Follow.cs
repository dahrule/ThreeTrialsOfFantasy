using UnityEngine;

[System.Serializable]
public class FollowOffset
{
    public Transform vrTarget;
    public Transform rigTarget;
    public Vector3 position;
    public Vector3 rotation;


    public void Map()
    {
        rigTarget.position = vrTarget.TransformPoint(position);
        rigTarget.rotation = vrTarget.rotation * Quaternion.Euler(rotation);
    }
}

public class Follow : MonoBehaviour
{
    public FollowOffset head;
    public FollowOffset leftHand;
    public FollowOffset rightHand;

    public Transform headConstraint;
    public Vector3 headBodyOffset;

    public float turnSmoothness = 3f;

    private void Start()
    {
       headBodyOffset = transform.position - headConstraint.position;
    }

    void LateUpdate()
    {
        transform.position = headConstraint.position + headBodyOffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized,Time.deltaTime*turnSmoothness);
        //transform.forward = Vector3.ProjectOnPlane(headConstraint.up, Vector3.up).normalized; //body moves instantly with head


        head.Map();
        leftHand.Map();
        rightHand.Map();
    }
}
