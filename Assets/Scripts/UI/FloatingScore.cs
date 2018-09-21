using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class FloatingScore : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private AnimationCurve _horizontalMovementCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

    private float __displacement;
    public float Displacement
    {
        get { return (this.__displacement); }
        set
        {
            this.__displacement = value;
            this._xStartPos = this._rectTransform.anchoredPosition.x;
            this._elapsedTime = 0.0f;
        }
    }

    private RectTransform _rectTransform;
    private float _xStartPos;
    private float _animationTime;
    private float _elapsedTime;
    private Vector2 _anchoredPosition;

    private void Awake()
    {
        this._rectTransform = this.GetComponent<RectTransform>();
        if (this._animator == null)
            Debug.LogWarning("_animator not assigned in " + this.name);
    }

    private void OnEnable()
    {
        this._xStartPos = this._rectTransform.anchoredPosition.x;
        AnimatorClipInfo[] clipInfo = this._animator.GetCurrentAnimatorClipInfo(0);
        this._animationTime = clipInfo[0].clip.length;
        Destroy(gameObject, this._animationTime);
        this._elapsedTime = 0.0f;
    }

    private void Update()
    {
        if (this._elapsedTime <= this._animationTime)
        {
            this._anchoredPosition = this._rectTransform.anchoredPosition;
            this._anchoredPosition.x = this._xStartPos + this._horizontalMovementCurve.Evaluate(this._elapsedTime / this._animationTime) * this.Displacement;
            this._rectTransform.anchoredPosition = this._anchoredPosition;
            this._elapsedTime += Time.deltaTime;
        }
    }
}
