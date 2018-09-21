using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateTextOpacity : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _text;
    [SerializeField] private AnimationCurve _animationCurve;

    public void AnimateOpacity()
    {
        this.StopAllCoroutines();
        this.StartCoroutine(this.Animate());
    }

    private IEnumerator Animate()
    {
        float animationTime = this._animationCurve.keys[this._animationCurve.length - 1].time;
        float elapsedTime = 0.0f;
        Color color = this._text.color;

        while (elapsedTime < animationTime)
        {
            color.a = this._animationCurve.Evaluate(elapsedTime);
            this._text.color = color;
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        color.a = this._animationCurve.Evaluate(animationTime);
        this._text.color = color;
        yield break;
    }
}
