using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInput : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private FloatVariable _temporaryInput;
    [SerializeField] private float _leftValue;
    [SerializeField] private float _rightValue;
    [SerializeField] private float _emptyValue;

    [Header("Events")]
    [SerializeField] private GameEvent _leftInputEvent;
    [SerializeField] private GameEvent _rightInputEvent;

    private float _tmp;

    private void Awake()
    {
        this._temporaryInput.Set(this._emptyValue);
    }

    private void Update()
    {
        if (this._temporaryInput.Value != this._emptyValue)
        {
            this._tmp = this._temporaryInput.Value;
            this._temporaryInput.Set(this._emptyValue);
            if (this._tmp == this._leftValue)
                this._leftInputEvent.Raise();
            else if (this._tmp == this._rightValue)
                this._rightInputEvent.Raise();
        }
    }
}
