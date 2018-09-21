using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/DontDestroyTraining", fileName = "new Training saver")]
public class DontDestroyTraining : ScriptableObject
{
    public string Text;
    public Newtonsoft.Json.Linq.JObject Value;
}
