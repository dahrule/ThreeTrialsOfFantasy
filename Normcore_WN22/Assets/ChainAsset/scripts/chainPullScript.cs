using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Normal.Realtime;
using System;

public class chainPullScript: RealtimeComponent<DoorAnimModel>
{
    [SerializeField]
    private float threshold = 0.1f;
    [SerializeField]
    private float deadZone = 0.025f;

    private bool _isPressed;
    private Vector3 _startPos;
    private ConfigurableJoint _joint;

    public UnityEvent onPressed, onReleased;

    [SerializeField]
    Animator wall;

    [SerializeField] AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        _startPos = transform.localPosition;
        _joint = GetComponent<ConfigurableJoint>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isPressed && GetValue() + threshold >= 1) Pressed();
        if (_isPressed && GetValue() - threshold <= 0) Released();
         //Debug.Log(GetValue());
        //Debug.Log(transform.localPosition);
    }

    protected override void OnRealtimeModelReplaced(DoorAnimModel previousModel, DoorAnimModel currentModel)
    {
        if (previousModel != null)
        {
            previousModel.openCloseDoorDidChange -= doorChange;
        }

        if (currentModel != null)
        {
            if (currentModel.isFreshModel)
            {
                model.openCloseDoor = 0;
            }
            currentModel.openCloseDoorDidChange += doorChange;
        }
    }

    private void doorChange(DoorAnimModel model, int value)
    {
       wall.SetInteger("DoorInt", value);
       audioSource.Play();

    }

    private float GetValue()
    {
        var value = Vector3.Distance(_startPos, transform.localPosition) / _joint.linearLimit.limit;

        if (Mathf.Abs(value) < deadZone)
            value = 0;

        return Mathf.Clamp(value, -1f, 1f);

    }

    public void Pressed()
    {
        _isPressed = true;
        //onPressed.Invoke();
      
        model.openCloseDoor = 1;
        Debug.Log("pressed");
    }

    private void Released()
    {
        _isPressed = false;
        // onReleased.Invoke();
        
        model.openCloseDoor = 0;
        Debug.Log("Released");
    }
}
