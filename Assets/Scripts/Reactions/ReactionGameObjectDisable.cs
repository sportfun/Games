using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReactionGameObjectDisable : MonoBehaviour
{
    [SerializeField] private float _timeBeforeReaction = 0.0f;
    [SerializeField] private GameObject _gameObject;

    public void React()
    {
        this.Invoke("Reaction", this._timeBeforeReaction);
    }

    private void Reaction()
    {
        this._gameObject.SetActive(false);
    }
}
