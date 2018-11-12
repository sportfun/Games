using System;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    [Serializable] public class InputEvent : UnityEvent {}
    [Serializable] public class SpeedEvent : UnityEvent<float> {}


    #region Unity Editor Field

    [Header("Input")]
    [SerializeField] private int _leftValue = 0;
    [SerializeField] private int _rightValue = 1;
    [Space(5)]
    [SerializeField] private InputEvent _leftInputEvent;
    [SerializeField] private InputEvent _rightInputEvent;
    
    [Header("Speed")]
    [SerializeField] private FloatVariable _minSpeed;
    [SerializeField] private FloatVariable _maxSpeed;
    [Space(5)]
    [SerializeField] private SpeedEvent _speedChangeEvent;

    #endregion

    public void OnDataReceivedHandler(string module, object data)
    {
        switch (module)
        {
            case "rpm":
                _speedChangeEvent.Invoke(Mathf.Clamp((float) data, _minSpeed.Value, _maxSpeed.Value));
                break;
            case "controller":
                if ((int) data == _leftValue) _leftInputEvent.Invoke();
                else if ((int) data == _rightValue) _rightInputEvent.Invoke();
                break;
        }
    }
}
