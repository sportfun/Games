using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshFollow : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private GameEvent _newLapEvent;
    [SerializeField] private GameEvent _endRaceEvent;
    [SerializeField] private List<Transform> _checkpoints = new List<Transform>();
    [SerializeField] private float _distanceBeforeSwitchTargetCheckpoint = 5.0f;
    [SerializeField] private FloatVariable _userSpeedVariable;
    [SerializeField] private FloatVariable _userOptimalSpeedVariable;
    [SerializeField] private float _optimalMoveSpeed;
    [SerializeField] private int _turns;

    private int _targetCheckpointIndex = 0;
    private int _currentTurn = 0;
    private bool _moving = false;

    private void Awake()
    {
        if (this._agent == null)
        {
            Debug.Log("Agent is missing!! - Trying to use the one attached");
            this._agent = this.GetComponent<NavMeshAgent>();
        }
    }

    private void OnEnable()
    {
        this._moving = true;
        this._agent.isStopped = false;
        this.InvokeRepeating("ChangeDestination", 0.0f, 0.5f);
    }

    private void OnDisable()
    {
        this.StopMoving();
        this.CancelInvoke();
    }

    private void Update()
    {
        this._agent.speed = this._userSpeedVariable.Value / (this._userOptimalSpeedVariable.Value / this._optimalMoveSpeed);
    }

    private void ChangeDestination()
    {
        if (Vector3.Distance(this.transform.position, this._checkpoints[this._targetCheckpointIndex].position) <= this._distanceBeforeSwitchTargetCheckpoint)
        {
            if (this._targetCheckpointIndex + 1 < this._checkpoints.Count)
                ++this._targetCheckpointIndex;
            else
                this._targetCheckpointIndex = 0;

            if (this._targetCheckpointIndex == 1)
            {
                ++this._currentTurn;
                if (this._currentTurn > 1)
                {
                    // if (this._currentTurn <= this._turns)
                        this._newLapEvent.Raise();
                    // else
                    // {
                    //     this.Invoke("StopMoving", 1.0f);
                    //     this._endRaceEvent.Raise();
                    // }
                }
            }
            if (this._moving)
                this._agent.SetDestination(this._checkpoints[this._targetCheckpointIndex].position);
        }
    }

    private void StopMoving()
    {
        if (this._agent.enabled)
            this._agent.isStopped = true;
        this._moving = false;
    }
}
