using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ColoredSliderMaterialUpdater : MonoBehaviour
{
    [SerializeField] private FloatVariable _goodSpeedDelta;
    [SerializeField] private FloatVariable _optimalUserSpeed;
    [SerializeField] private FloatVariable _minSpeed;
    [SerializeField] private FloatVariable _maxSpeed;

    private Material _material;

    private void Awake()
    {
        Image image = this.GetComponent<Image>();
        this._material = image.material;
    }

    private void OnEnable()
    {
        this.UpdateMaterialValues();
    }

    public void UpdateMaterialValues()
    {
        this._material.SetFloat("_Value", Mathf.Clamp01(this._goodSpeedDelta.Value * 2.0f / (this._maxSpeed.Value - this._minSpeed.Value)));
    }

    public void UpdateOptimalSpeed()
    {
        this._material.SetFloat("_Position", Mathf.Clamp(Utility.ScaleValue(this._optimalUserSpeed.Value, 0.1f, 0.9f, this._minSpeed.Value, this._maxSpeed.Value), 0.1f, 0.9f));
    }
}
