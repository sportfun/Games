using UnityEngine;

public enum TrainingType { SPRINT, ENDURANCE, FRACTION, RESTING }

public class TrainingSequence
{
    public TrainingType Type
    {
        get
        {
            if (this._resting)
                return (TrainingType.RESTING);
            else
                return (this._type);
        }
        set
        {
            this._type = value;
        }
    }

    public float TotalLength;
    public float EffortLength;
    public float RestLength;
    public float Iterations;

    private TrainingType _type;
    private float _elapsedTime;
    private bool _resting;

    public TrainingSequence(TrainingType type, float totalLength, float effortLength, float restLength, int iterations)
    {
        this.Type = type;
        this.TotalLength = totalLength * 60.0f;
        this.EffortLength = effortLength * 60.0f;
        this.RestLength = restLength * 60.0f;
        this.Iterations = iterations;
        this.Init();
    }

    public void Init()
    {
        this._elapsedTime = 0.0f;
        this._resting = false;
    }

    public void IncreaseElapsedTime(float elapsedTime)
    {
        this._elapsedTime += elapsedTime;
        if (this._elapsedTime > this.EffortLength + this.RestLength)
            this._elapsedTime = 0.0f;
        if (this._elapsedTime < this.EffortLength)
            this._resting = false;
        if (this._elapsedTime >= this.EffortLength)
            this._resting = true;
    }
}
