using UnityEngine;

public class LapHandler : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _lapText;

    [SerializeField] private IntVariable _currentLapVariable;
    [SerializeField] private IntVariable _lapCountVariable;

    public void ChangeLapText()
    {
        // if (this._currentLapVariable.Value > this._lapCountVariable.Value)
        //     this._currentLapVariable.Set(this._lapCountVariable.Value);
        this._lapText.SetText("Tour " + this._currentLapVariable.Value);
    }
}
