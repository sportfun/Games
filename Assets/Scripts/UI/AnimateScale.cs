using System.Collections;
using UnityEngine;

public class AnimateScale : MonoBehaviour
{
    [SerializeField] private AnimationCurve _animationCurve;
    [SerializeField] private bool _applyOnX;
    [SerializeField] private bool _applyOnY;

    private RectTransform _rectTransform;

    private void Awake()
    {
        this._rectTransform = this.GetComponent<RectTransform>();
        Vector3 scale = this._rectTransform.localScale;
        if (this._applyOnX)
            scale.x = this._animationCurve.Evaluate(0.0f);
        if (this._applyOnY)
            scale.y = this._animationCurve.Evaluate(0.0f);
        this._rectTransform.localScale = scale;
    }

    public void ScaleUp()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Animate(true));
    }

    public void ScaleDown()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Animate(false));
    }

    private IEnumerator Animate(bool upScale)
    {
        float animationTime = this._animationCurve.keys[this._animationCurve.length - 1].time;
        float elapsedTime = 0;
        float tmp = this._rectTransform.localScale.y;
        Vector3 scale = this._rectTransform.localScale;
        float scaleFrom = (upScale) ? this._animationCurve.Evaluate(0.0f) : this._animationCurve.Evaluate(animationTime);
        float scaleTo = (upScale) ? this._animationCurve.Evaluate(animationTime) : this._animationCurve.Evaluate(0.0f);

        if ((this._applyOnX && scale.x == scaleTo) || (this._applyOnY && scale.y == scaleTo))
            yield break;

        while (elapsedTime <= animationTime)
        {
            tmp = Mathf.Lerp(scaleFrom, scaleTo, this._animationCurve.Evaluate(elapsedTime));
            this._rectTransform.localScale = new Vector3((this._applyOnX ? tmp : scale.x), (this._applyOnY ? tmp : scale.y), scale.z);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        this._rectTransform.localScale = new Vector3((this._applyOnX ? scaleTo : scale.x), (this._applyOnY ? scaleTo : scale.y), scale.z);
        yield break;
    }
}
