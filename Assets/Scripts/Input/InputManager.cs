using System;
using Newtonsoft.Json.Linq;
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
    [SerializeField] private InputEvent _leftInputEvent = new InputEvent();
    [SerializeField] private InputEvent _rightInputEvent = new InputEvent();
    
    [Header("Speed")]
    [SerializeField] private FloatVariable _minSpeed;
    [SerializeField] private FloatVariable _maxSpeed;
    [Space(5)]
    [SerializeField] private SpeedEvent _speedChangeEvent = new SpeedEvent();

    #endregion

    public void OnDataReceivedHandler(string module, object data)
    {
        switch (module)
        {
            case "rpm":
                var speed = (data as JValue)?.ToObject<float>() ?? (float) data;
                _speedChangeEvent.Invoke(Mathf.Clamp(speed, _minSpeed.Value, _maxSpeed.Value));
                break;
            case "controller":
                var input = (data as JValue)?.ToObject<int>() ?? (int) data;
                if (input == _leftValue) _leftInputEvent.Invoke();
                else if (input == _rightValue) _rightInputEvent.Invoke();
                break;
        }
    }
}
