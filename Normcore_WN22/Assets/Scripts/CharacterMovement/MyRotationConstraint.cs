using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// Sets this object's position to the parent's + an offset. Rotation is ignored.
///
public class MyRotationConstraint : MonoBehaviour
{
    [SerializeField] Transform parent;
    [SerializeField] Vector3  parentOffset;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parent.position+parentOffset;
        //transform.rotation = Quaternion.Euler(new Vector3(parent.rotation.x, parent.rotation.y, parent.rotation.z));
        //transform.localEulerAngles = new Vector3(parent.localEulerAngles.x, parent.localEulerAngles.y,parent.localEulerAngles.z);
    }
}
