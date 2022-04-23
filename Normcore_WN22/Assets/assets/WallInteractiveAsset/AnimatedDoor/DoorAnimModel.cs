using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Normal.Realtime.Serialization;
using Normal.Realtime;


[RealtimeModel]
public partial class DoorAnimModel 
{

    [RealtimeProperty(1, false, true)] private bool _openCloseDoor;

}

/* ----- Begin Normal Autogenerated Code ----- */
public partial class DoorAnimModel : RealtimeModel {
    public bool openCloseDoor {
        get {
            return _openCloseDoorProperty.value;
        }
        set {
            if (_openCloseDoorProperty.value == value) return;
            _openCloseDoorProperty.value = value;
            InvalidateUnreliableLength();
            FireOpenCloseDoorDidChange(value);
        }
    }
    
    public delegate void PropertyChangedHandler<in T>(DoorAnimModel model, T value);
    public event PropertyChangedHandler<bool> openCloseDoorDidChange;
    
    public enum PropertyID : uint {
        OpenCloseDoor = 1,
    }
    
    #region Properties
    
    private UnreliableProperty<bool> _openCloseDoorProperty;
    
    #endregion
    
    public DoorAnimModel() : base(null) {
        _openCloseDoorProperty = new UnreliableProperty<bool>(1, _openCloseDoor);
    }
    
    private void FireOpenCloseDoorDidChange(bool value) {
        try {
            openCloseDoorDidChange?.Invoke(this, value);
        } catch (System.Exception exception) {
            UnityEngine.Debug.LogException(exception);
        }
    }
    
    protected override int WriteLength(StreamContext context) {
        var length = 0;
        length += _openCloseDoorProperty.WriteLength(context);
        return length;
    }
    
    protected override void Write(WriteStream stream, StreamContext context) {
        var writes = false;
        writes |= _openCloseDoorProperty.Write(stream, context);
        if (writes) InvalidateContextLength(context);
    }
    
    protected override void Read(ReadStream stream, StreamContext context) {
        var anyPropertiesChanged = false;
        while (stream.ReadNextPropertyID(out uint propertyID)) {
            var changed = false;
            switch (propertyID) {
                case (uint) PropertyID.OpenCloseDoor: {
                    changed = _openCloseDoorProperty.Read(stream, context);
                    if (changed) FireOpenCloseDoorDidChange(openCloseDoor);
                    break;
                }
                default: {
                    stream.SkipProperty();
                    break;
                }
            }
            anyPropertiesChanged |= changed;
        }
        if (anyPropertiesChanged) {
            UpdateBackingFields();
        }
    }
    
    private void UpdateBackingFields() {
        _openCloseDoor = openCloseDoor;
    }
    
}
/* ----- End Normal Autogenerated Code ----- */