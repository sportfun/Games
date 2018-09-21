using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    [Header("Physics")]
    [SerializeField] private LayerMask _obstacleLayerMask;
    [SerializeField] private LayerMask _bonusLayerMask;

    [Header("Properties")]
    [SerializeField] private float _offset = 2.0f;
    [SerializeField] private int _maxOffset;
    [SerializeField] private AnimationCurve _slideCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private FloatVariable _invicibilityTimeAfterDeath;

    [Header("Events")]
    [SerializeField] private GameEvent _deathEvent;
    [SerializeField] private GameEvent _bonusEvent;

    private Vector3 _originPosition;
    private bool _moving;
    private int _moves;
    private float _elapsedTime;

    private void Awake()
    {
        this._originPosition = this.transform.position;
        this._moving = false;
        this._moves = 0;
        this._elapsedTime = this._invicibilityTimeAfterDeath.Value;
    }

    private void Update()
    {
        if (this._elapsedTime <= this._invicibilityTimeAfterDeath.Value)
            this._elapsedTime += Time.deltaTime;
    }

    public void OnMoveLeft()
    {
        if (this._moves > - this._maxOffset)
            this.Move(false);
    }

    public void OnMoveRight()
    {
        if (this._moves < this._maxOffset)
            this.Move(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this._elapsedTime < this._invicibilityTimeAfterDeath.Value)
            return;
        if (this._obstacleLayerMask.value == (this._obstacleLayerMask.value | 1 << other.gameObject.layer))
            this.OnObstacleCollision();
        else if (this._bonusLayerMask.value == (this._bonusLayerMask.value | 1 << other.gameObject.layer))
            this.OnBonusCollision(other.gameObject);
    }

    private void Move(bool right)
    {
        if (this._moving)
            return;
        this.StopAllCoroutines();
        this._moves += (right) ? 1 : -1;
        this.StartCoroutine(this.Co_Move(right));
    }

    private IEnumerator Co_Move(bool right)
    {
        this._moving = true;
        float time = this._slideCurve.keys[this._slideCurve.length - 1].time;
        float elapsedTime = 0.0f;
        Vector3 pos = this.transform.position;
        float startPosition = pos.x;

        while (elapsedTime <= time)
        {
            if (right)
                pos.x = startPosition + (this._slideCurve.Evaluate(elapsedTime) * this._offset);
            else
                pos.x = startPosition - (this._slideCurve.Evaluate(elapsedTime) * this._offset);
            this.transform.position = pos;
            yield return null;
            elapsedTime += Time.deltaTime;
        }
        if (right)
            pos.x = startPosition + this._offset;
        else
            pos.x = startPosition - this._offset;
        this.transform.position = pos;
        this._moving = false;
        yield break;
    }

    private void OnObstacleCollision()
    {
        this.StopAllCoroutines();
        this._deathEvent.Raise();
        this.transform.position = this._originPosition;
        this._elapsedTime = 0.0f;
        this._moves = 0;
        this._moving = false;
    }

    private void OnBonusCollision(GameObject bonus)
    {
        this._bonusEvent.Raise();
        Destroy(bonus);
    }
}
