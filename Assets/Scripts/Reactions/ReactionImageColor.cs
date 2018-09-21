using UnityEngine;
using UnityEngine.UI;

public class ReactionImageColor : MonoBehaviour
{
    [SerializeField] private float _timeBeforeReaction = 0.0f;
    [SerializeField] private Image _image;
    [SerializeField] private Color _color;

    private void Awake()
    {
        if (this._image == null)
            this._image = this.GetComponent<Image>();
    }

    public void React()
    {
        if (this._image == null)
            Debug.LogWarning("_image field in " + this.name + " is null");
        else
            this.Invoke("Reaction", this._timeBeforeReaction);
    }

    private void Reaction()
    {
        this._image.color = this._color;
    }
}
