using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageDisplay : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private TMPro.TextMeshProUGUI _textElement;
    [SerializeField] private CanvasGroup _canvasGroupElement;
    [SerializeField] private AnimationCurve _displayCurve;
    [SerializeField] private Color _goodColor = Color.green;
    [SerializeField] private Color _badColor = Color.red;


    [Header("Texts")]
    [SerializeField] private string _slowText;
    [SerializeField] private string _goodText;
    [SerializeField] private string _fastText;

    private float _displayTimer;
    private float _animationTime;

    private void Awake()
    {
        this._displayTimer = float.MaxValue;
        this._canvasGroupElement.alpha = 0.0f;
        this._animationTime = this._displayCurve.keys[this._displayCurve.length - 1].time;
    }

    private void Update()
    {
        if (this._displayTimer <= this._animationTime)
        {
            this._displayTimer += Time.deltaTime;
            this._canvasGroupElement.alpha = this._displayCurve.Evaluate(this._displayTimer);
        }
        else
            this._canvasGroupElement.alpha = 0.0f;
    }

    public void OnTooSlow()
    {
        this._textElement.color = this._badColor;
        this._textElement.SetText(this._slowText);
        this._displayTimer = 0.0f;
    }

    public void OnGoodSpeed()
    {
        this._textElement.color = this._goodColor;
        this._textElement.SetText(this._goodText);
        this._displayTimer = 0.0f;
    }

    public void OnTooFast()
    {
        this._textElement.color = this._badColor;
        this._textElement.SetText(this._fastText);
        this._displayTimer = 0.0f;
    }
}
