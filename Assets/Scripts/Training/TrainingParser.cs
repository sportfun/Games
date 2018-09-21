using System.Collections.Generic;
using UnityEngine;

public class TrainingParser
{
    private class Sequence
    {
        public int type;
        public float totalLength;
        public float effortLength;
        public float restLength;
        public int iteration;
    }

    private class Data
    {
        public IList<Sequence> sequences;
    }

    private class JsonTraining
    {
        public Data data;
    }

    static public List<TrainingSequence> ParseText(string text)
    {
        JsonTraining obj = Newtonsoft.Json.JsonConvert.DeserializeObject<JsonTraining>(text);
        List<TrainingSequence> sequences = new List<TrainingSequence>();

        for (int i = 0; i < obj.data.sequences.Count; ++i)
            sequences.Add(new TrainingSequence((TrainingType) (obj.data.sequences[i].type - 1), obj.data.sequences[i].totalLength, obj.data.sequences[i].effortLength, obj.data.sequences[i].restLength, obj.data.sequences[i].iteration));

        return (sequences);
    }
}
