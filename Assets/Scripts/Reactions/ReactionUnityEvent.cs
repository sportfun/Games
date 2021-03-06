﻿using UnityEngine;
using UnityEngine.Events;

public class ReactionUnityEvent : MonoBehaviour
{
    [SerializeField] private float _timeBeforeReaction = 0.0f;
    [SerializeField] private UnityEvent _preReactionEvent;
    [SerializeField] private UnityEvent _event;

    public void React()
    {
        this._preReactionEvent.Invoke();
        this.Invoke("Reaction", this._timeBeforeReaction);
    }

    private void Reaction()
    {
        this._event.Invoke();
    }
}
