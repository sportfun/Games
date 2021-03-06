﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionParticleSystem : MonoBehaviour
{
    [SerializeField] private float _timeBeforeReaction = 0.0f;
    [SerializeField] private ParticleSystem _particleSystem;

    public void React()
    {
        this.Invoke("Reaction", this._timeBeforeReaction);
    }

    private void Reaction()
    {
        this._particleSystem.Play();
    }
}
