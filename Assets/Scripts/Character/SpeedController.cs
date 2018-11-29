using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpeedState { SLOW, GOOD, FAST }

public class SpeedController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private FloatVariable _userSpeedVariable;
    [SerializeField] private FloatVariable _userOptimalSpeedVariable;
    [SerializeField] private FloatVariable _goodSpeedDelta;
    [SerializeField] private float _timeBetweenStayGoodRewards = 1.0f;
    [SerializeField] private float _timeBetweenStayBadPenalty = 1.0f;

    [Header("Events")]
    [SerializeField] private GameEvent _tooSlowEvent;
    [SerializeField] private GameEvent _tooFastEvent;
    [SerializeField] private GameEvent _goodSpeedEvent;
    [SerializeField] private GameEvent _stayGoodSpeedEvent;
    [SerializeField] private GameEvent _stayBadSpeedEvent;

    private SpeedState __speedState;
    private SpeedState _speedState
    {
        get { return (this.__speedState); }
        set
        {
            if (this.__speedState == value)
                return;
            this._elapsedTime = 0.0f;
            this.__speedState = value;
            if (value == SpeedState.SLOW)
                this._tooSlowEvent.Raise();
            else if (value == SpeedState.FAST)
                this._tooFastEvent.Raise();
            else
                this._goodSpeedEvent.Raise();
        }
    }
    private float _elapsedTime;

    private void OnEnable()
    {
        this._speedState = SpeedState.GOOD;
        this.OnUserSpeedChange();
    }

    private void Update()
    {
        if (this._speedState == SpeedState.GOOD)
        {
            if (this._elapsedTime >= this._timeBetweenStayGoodRewards)
            {
                this._elapsedTime -= this._timeBetweenStayGoodRewards;
                this._stayGoodSpeedEvent.Raise();
            }
        }
        else
        {
            if (this._elapsedTime >= this._timeBetweenStayBadPenalty)
            {
                this._elapsedTime -= this._timeBetweenStayBadPenalty;
                this._stayBadSpeedEvent.Raise();
            }
        }
        this._elapsedTime += Time.deltaTime;
    }

    public void SetUserSpeed(float speed)
    {
        _userSpeedVariable.Set(speed);
    }

    public void OnUserSpeedChange(float speed)
    {
        _userSpeedVariable.Set(speed);
        OnUserSpeedChange();
    }
    
    public void OnUserSpeedChange()
    {
        float delta = this._userSpeedVariable.Value - this._userOptimalSpeedVariable.Value;
        if (Mathf.Abs(delta) < this._goodSpeedDelta.Value)
        {
            this._speedState = SpeedState.GOOD;
        }
        else
        {
            if (delta < 0.0f)
                this._speedState = SpeedState.SLOW;
            else
                this._speedState = SpeedState.FAST;
        }
    }
}
