using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> {}

public class TrainingHandler : MonoBehaviour
{
    [SerializeField] private bool _parseTrainingOnAwake;
    [SerializeField] private DontDestroyTraining _trainingData;

    [Header("Events")]
    [SerializeField] private GameEvent _finishedTrainingEvent;
    [SerializeField] private UnityFloatEvent _changeOptimalUserSpeedEvent;

    [Header("Speed values")]
    [SerializeField] private FloatVariable _enduranceSpeed;
    [SerializeField] private FloatVariable _sprintSpeed;
    [SerializeField] private FloatVariable _fractionSpeed;
    [SerializeField] private FloatVariable _restSpeed;

    private List<TrainingSequence> _currentTraining;
    private int _currentSequenceIndex = 0;
    private float _elapsedTime = 0.0f;
    private bool _finished = true;

    private void Awake()
    {
        if (this._parseTrainingOnAwake)
        {
            this.AcquireTraining(this._trainingData.Text);
        }
    }

    public void AcquireTraining(string jsonText)
    {
        this._currentTraining = TrainingParser.ParseText(jsonText);
    }

    public void StartTraining()
    {
        if (this._currentTraining == null || this._currentTraining.Count == 0)
            Debug.LogWarning("!! _currentTraining is null or empty !!");
        this._finished = false;
        this._currentSequenceIndex = 0;
        this._currentTraining[this._currentSequenceIndex].Init();
        this._changeOptimalUserSpeedEvent.Invoke(this.ChooseSpeed(this._currentTraining[this._currentSequenceIndex].Type));
    }

    private void Update()
    {
        if (this._finished)
            return;
        this._elapsedTime += Time.deltaTime;
        if (this._elapsedTime >= this._currentTraining[this._currentSequenceIndex].TotalLength)
            this.ChangeSequence();
        else
            this.CheckIteration();
    }

    private void ChangeSequence()
    {
        this._elapsedTime = 0.0f;
        if (this._currentSequenceIndex + 1 < this._currentTraining.Count)
        {
            // Change sequence, continue training
            ++this._currentSequenceIndex;
            this._currentTraining[this._currentSequenceIndex].Init();
            this._changeOptimalUserSpeedEvent.Invoke(this.ChooseSpeed(this._currentTraining[this._currentSequenceIndex].Type));
            Debug.Log("Changing to sequence #" + this._currentSequenceIndex);
        }
        else
        {
            // Last sequence done, stop training
            this._finished = true;
            this._finishedTrainingEvent.Raise();
        }
    }

    private void CheckIteration()
    {
        if (this._currentTraining[this._currentSequenceIndex].Iterations == 0)
            return;

        this._currentTraining[this._currentSequenceIndex].IncreaseElapsedTime(Time.deltaTime);
        this._changeOptimalUserSpeedEvent.Invoke(this.ChooseSpeed(this._currentTraining[this._currentSequenceIndex].Type));
    }

    private float ChooseSpeed(TrainingType trainingType)
    {
        if (trainingType == TrainingType.ENDURANCE)
            return (this._enduranceSpeed.Value);
        else if (trainingType == TrainingType.SPRINT)
            return (this._sprintSpeed.Value);
        else if (trainingType == TrainingType.FRACTION)
            return (this._fractionSpeed.Value);
        else
            return (this._restSpeed.Value);
    }
}
