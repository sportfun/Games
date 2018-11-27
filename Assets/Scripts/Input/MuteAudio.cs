using UnityEngine;
using UnityEngine.Audio;

public class MuteAudio : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _volumeButtonText;
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private AudioMixerSnapshot[] _audioSnapshots;

    private bool _muted;

    private void OnEnable()
    {
        this._muted = (PlayerPrefs.GetInt("AudioMuted", 0) == 0 ? true : false);
        this.SetAudio();
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void ToggleAudio()
    {
        this._muted = !this._muted;

        this.SetAudio();
    }

    public void SetAudio()
    {
        float[] weights = new float[2];
        weights[0] = this._muted ? 0.0f : 1.0f;
        weights[1] = this._muted ? 1.0f : 0.0f;
        this._audioMixer.TransitionToSnapshots(this._audioSnapshots, weights, 0.1f);

        PlayerPrefs.SetInt("AudioMuted", this._muted ? 1 : 0);
        this.UpdateText();
    }

    private void UpdateText()
    {
        this._volumeButtonText.SetText(this._muted ? "h" : "g");
    }
}
