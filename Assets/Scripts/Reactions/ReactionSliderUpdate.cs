using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactionSliderUpdate : MonoBehaviour
{
    [SerializeField] private float _timeBeforeReaction = 0.0f;
    [SerializeField] private Slider _slider;
    [SerializeField] private FloatVariable _newValue;

    private void Awake()
    {
        if (this._slider == null)
            this._slider = this.GetComponent<Slider>();
    }

    public void React()
    {
        if (this._slider == null)
            Debug.LogWarning("_slider field in " + this.name + " is null");
        else
            this.Invoke("Reaction", this._timeBeforeReaction);
    }

    private void Reaction()
    {
        this._slider.value = this._newValue.Value;
    }
}
