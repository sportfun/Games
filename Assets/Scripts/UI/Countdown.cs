using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> {} 

public class Countdown : MonoBehaviour
{
    [SerializeField] private int _countdownFrom;
    [SerializeField] private UnityEvent _countdownStart;
    [SerializeField] private UnityStringEvent _countdownEvent;
    [SerializeField] private UnityEvent _countdownOverEvent;

    private float _elapsedTime;
    private int _currentCount;

    private void OnEnable()
    {
        this._elapsedTime = 0.0f;
        this._currentCount = this._countdownFrom;
        this._countdownStart.Invoke();
        this._countdownEvent.Invoke(this._currentCount.ToString());
    }

    private void Update()
    {
        this._elapsedTime += Time.deltaTime;

        if (this._elapsedTime >= 1.0f)
        {
            this._elapsedTime -= 1.0f;
            --this._currentCount;
            if (this._currentCount > 0)
                this._countdownEvent.Invoke(this._currentCount.ToString());
            else if (this._currentCount == 0)
                this._countdownOverEvent.Invoke();
        }
    }
}
