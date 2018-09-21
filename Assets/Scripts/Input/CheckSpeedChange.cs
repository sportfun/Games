using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSpeedChange : MonoBehaviour
{
    [SerializeField] private FloatVariable _temporarySpeed;
    [SerializeField] private FloatVariable _actualSpeed;
    [SerializeField] private GameEvent _speedChangeEvent;
    [SerializeField] private FloatVariable _minSpeed;
    [SerializeField] private FloatVariable _maxSpeed;

    private void Awake()
    {
        this._temporarySpeed.Set(this._minSpeed.Value);
    }

    private void Update()
    {
        if (this._temporarySpeed.Value != this._actualSpeed.Value)
        {
            this._actualSpeed.Set(Mathf.Clamp(this._temporarySpeed.Value, this._minSpeed.Value, this._maxSpeed.Value));
            this._speedChangeEvent.Raise();
        }
    }
}
