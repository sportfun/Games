using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeRaiser : MonoBehaviour
{
    [SerializeField] private GameEvent _onAwakeEvent;

    private void Awake()
    {
        this._onAwakeEvent.Raise();
    }
}
