using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private GameEvent _pauseEvent;

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
            this._pauseEvent.Raise();
    }

    private void OnApplicationPause(bool value)
    {
        if (value)
            this._pauseEvent.Raise();
    }
}
