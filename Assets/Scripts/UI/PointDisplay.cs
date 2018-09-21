using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointDisplay : MonoBehaviour
{
    [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    [SerializeField] private int _randomPosDelta = 10;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;
    private Vector3 _pos;
    private float _timer;
    private Vector3 _tmpPos;
    private Vector3 _targetPos;

    private void Awake()
    {
        this._canvasGroup = this.GetComponent<CanvasGroup>();
        this._rectTransform = this.GetComponent<RectTransform>();
        this._pos = this._rectTransform.localPosition;
        this._tmpPos = this._pos;
        this._timer = float.MaxValue;
    }

    private void Update()
    {
        if (this._timer <= this._fadeCurve.keys[this._fadeCurve.length - 1].time)
        {
            this._timer += Time.deltaTime;
            this._canvasGroup.alpha = this._fadeCurve.Evaluate(this._timer);
        }
        else
            this._canvasGroup.alpha = 0.0f;
    }

    public void Emit()
    {
        this._tmpPos.x = Random.Range(this._pos.x - (float)this._randomPosDelta / 2.0f, this._pos.x + (float)this._randomPosDelta / 2.0f);
        this._tmpPos.y = Random.Range(this._pos.y - (float)this._randomPosDelta / 2.0f, this._pos.y + (float)this._randomPosDelta / 2.0f);

        this._rectTransform.localPosition = this._tmpPos;
        this._timer = 0.0f;
    }
}
