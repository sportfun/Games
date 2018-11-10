using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AwakeRaiser : MonoBehaviour
{
    [SerializeField] private GameEvent _onAwakeEvent;
    [SerializeField] private UnityEvent _onAvakeUnityEvent;

    private void Awake()
    {
        if (this._onAwakeEvent != null)
            this._onAwakeEvent.Raise();
        this._onAvakeUnityEvent.Invoke();
    }
}
