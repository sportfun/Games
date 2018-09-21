using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float", fileName = "New float variable")]
public class FloatVariable : ScriptableObject
{
    public float Value;

    public void Increment(float value)
    {
        this.Value += value;
    }

    public void Decrement(float value)
    {
        this.Value -= value;
    }

    public void Set(float value)
    {
        this.Value = value;
    }
}
