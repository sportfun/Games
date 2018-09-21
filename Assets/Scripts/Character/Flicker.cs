using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private FloatVariable _flickeringDuration;
    [SerializeField] private GameObject _flicker;

    private float _elapsedTime;

    private void OnEnable()
    {
        this._elapsedTime = 0.0f;
    }

    private void OnDisable()
    {
        this._flicker.SetActive(true);
    }

    private void Update()
    {
        this._elapsedTime += Time.deltaTime;
        if (this._elapsedTime >= this._flickeringDuration.Value)
        {
            this.enabled = false;
            return;
        }
        if (this._animationCurve.Evaluate(this._elapsedTime) > 0.5f)
            this._flicker.SetActive(true);
        else
            this._flicker.SetActive(false);
    }
}
