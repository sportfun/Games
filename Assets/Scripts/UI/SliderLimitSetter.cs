using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderLimitSetter : MonoBehaviour
{
    [SerializeField] private FloatVariable _min;
    [SerializeField] private FloatVariable _max;

    private void Awake()
    {
        Slider slider = this.GetComponent<Slider>();

        slider.maxValue = this._max.Value;
        slider.minValue = this._min.Value;
    }
}
