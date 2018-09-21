using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private IntVariable _scoreVariable;

    private TextMeshProUGUI _scoreText;

    private void Awake()
    {
        this._scoreText = this.GetComponent<TextMeshProUGUI>();
    }

    public void OnNeedUpdate()
    {
        this._scoreText.SetText(this._scoreVariable.Value.ToString());
    }
}
