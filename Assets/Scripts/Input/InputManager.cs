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

    public void OnDataReceivedHandler(string channel, JObject json)
    {
        if (channel != "data") return;
        
        if (json?["body"]?["value"] == null)
        {
            Debug.LogError($"Socket.io: received invalid data: {json}");
            return;
        }

        Debug.Log($"Socket.io: data received: {json}");
        switch ((string) json["body"]["module"])
        {
            case "rpm":
            case "controller":
                var module = (string) json["body"]["module"];
                var value = json["body"]["value"];

                switch (module)
                {
                    case "rpm":
                        var speed = (value as JValue)?.ToObject<float>() ?? -1;
                        _speedChangeEvent.Invoke(Mathf.Clamp(speed, _minSpeed.Value, _maxSpeed.Value));
                        break;
                    case "controller":
                        var input = (value as JValue)?.ToObject<int>() ?? -1;
                        if (input == _leftValue) _leftInputEvent.Invoke();
                        else if (input == _rightValue) _rightInputEvent.Invoke();
                        break;
                }

                break;
            case null:
                Debug.LogError("Socket.io: module not defined");
                break;
            default:
                Debug.LogWarning($"Socket.io: unknown module '{json["body"]["module"]}'");
                break;
        }

        
    }
}
