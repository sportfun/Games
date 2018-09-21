using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/int", fileName = "New int variable")]
public class IntVariable : ScriptableObject
{
    public int Value;

    public void Increment(int value)
    {
        this.Value += value;
    }

    public void Decrement(int value)
    {
        this.Value -= value;
    }

    public void Set(int value)
    {
        this.Value = value;
    }
}
