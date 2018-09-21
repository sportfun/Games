using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputDispatcher : MonoBehaviour
{
    [SerializeField] private GameEvent _onLeftInput;
    [SerializeField] private GameEvent _onRightInput;

    private void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            if (Input.GetAxis("Horizontal") > 0)
                this._onRightInput.Raise();
            if (Input.GetAxis("Horizontal") < 0)
                this._onLeftInput.Raise();
        }
    }
}
