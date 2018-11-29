using UnityEngine;

public class PauseInput : MonoBehaviour
{
    [SerializeField] private GameEvent _pauseEvent;

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
            this._pauseEvent.Raise();
    }

#if !UNITY_EDITOR
    private void OnApplicationPause(bool value)
    {
        if (value)
            this._pauseEvent.Raise();
    }
#endif
}
