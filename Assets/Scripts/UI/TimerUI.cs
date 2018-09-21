using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private float _elapsedTime;

    private void Awake()
    {
        this._elapsedTime = 0.0f;
        this.SetTimeText();
    }

    private void Update()
    {
        this._elapsedTime += Time.deltaTime;
        this.SetTimeText();
    }

    private void SetTimeText()
    {
        this._text.SetText(Mathf.Floor(this._elapsedTime / 60.0f).ToString("#0") + ':' + Mathf.Floor(this._elapsedTime % 60.0f).ToString("00"));
    }

    public float GetTime()
    {
        return (this._elapsedTime);
    }
}
