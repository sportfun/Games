using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    [SerializeField] private AnimationCurve _fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float _fadeTime;
    [SerializeField] private UnityEvent _onFadeInCompleteEvent;
    [SerializeField] private UnityEvent _onFadeOutCompleteEvent;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        this._canvasGroup = this.GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        this.StartCoroutine(this.AnimateFade(true));
    }

    public void FadeOut()
    {
        this.StartCoroutine(this.AnimateFade(false));
    }

    private IEnumerator AnimateFade(bool fadeIn)
    {
        float elapsedTime = 0.0f;

        if ((fadeIn && this._canvasGroup.alpha == 1.0f) || (!fadeIn && this._canvasGroup.alpha == 0.0f))
            yield break;

        while (elapsedTime <= this._fadeTime)
        {
            // if (GameConstants.paused)
            // {
            //     yield return null;
            //     continue;
            // }
            if (fadeIn)
                this._canvasGroup.alpha = this._fadeCurve.Evaluate(elapsedTime / this._fadeTime);
            else
                this._canvasGroup.alpha = this._fadeCurve.Evaluate(1.0f - elapsedTime / this._fadeTime);
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        if (fadeIn)
        {
            this._canvasGroup.alpha = this._fadeCurve.Evaluate(1.0f);
            this._onFadeInCompleteEvent.Invoke();
        }
        else
        {
            this._canvasGroup.alpha = this._fadeCurve.Evaluate(0.0f);
            this._onFadeOutCompleteEvent.Invoke();
        }
        yield break;
    }
}
