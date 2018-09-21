using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImageColorOpacity : MonoBehaviour
{
    [SerializeField] private Image _image;
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
        Color color = this._image.color;

        while (elapsedTime < animationTime)
        {
            color.a = this._animationCurve.Evaluate(elapsedTime);
            this._image.color = color;
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        color.a = this._animationCurve.Evaluate(animationTime);
        this._image.color = color;
        yield break;
    }
}
