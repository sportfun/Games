using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DebugActions : MonoBehaviour
{
    [SerializeField] private List<string> _inputs;
    [SerializeField] private List<UnityEvent> _events;

    private void Update()
    {
        for (int i = 0; i < this._inputs.Count; ++i)
        {
            if (Input.GetButtonUp(this._inputs[i]))
            {
                this._events[i].Invoke();
            }
        }
    }
}
