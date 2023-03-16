using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    PINGPONG,
    CLOKWISE,
    COUNTERCLOCK,
    CHASING
}
public class EnemyPatrol : MonoBehaviour
{
    #region Expose
    [SerializeField] private Transform[] _waypoints;
    [SerializeField] private int _startingID;
    [SerializeField] private PatrolType _patrolMode = PatrolType.CLOKWISE;
    [SerializeField] private Color _colorGizmos;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _coneVision = GetComponentInChildren<ConeVision>();
    }

    void Start()
    {
        _startPatrolType = _patrolMode;
        _agent.Warp(_waypoints[_startingID].position);
        _agent.SetDestination(_waypoints[_startingID].position);
        _destinationID = _startingID;
    }

    void Update()
    {
        
            switch (_patrolMode)
            {
                case PatrolType.CLOKWISE:
                    ClockWise();
                    DetectPlayer();
                break;
                case PatrolType.COUNTERCLOCK:
                    CounterClockWise();
                    DetectPlayer();
                break;
                case PatrolType.PINGPONG:
                    PingPong();
                    DetectPlayer();
                    break;
                case PatrolType.CHASING:
                    Debug.Log("Je course le joueur");
                    if (_coneVision._target != null)
                    {
                        Chasing();
                    }
                    else
                    {
                        Debug.Log("Je courais après le joueur et je l'ai perdu");
                        _patrolMode = _startPatrolType;
                    }
                    break;
            }
    }

    private void OnDrawGizmos()
    {
        if (_waypoints == null || _waypoints.Length == 0)
        {
            return;
        }
        Gizmos.color = _colorGizmos;
        for (int i = 0; i < _waypoints.Length; i++)
        {
            if (i == 0)
            {
                Gizmos.DrawSphere(_waypoints[i].position, 1);
            }
            if(i == _waypoints.Length - 1) //si je suis au dernier
            {
                Gizmos.DrawSphere(_waypoints[i].position, 1);
                if (_patrolMode != PatrolType.PINGPONG)
                {
                    Gizmos.DrawLine(_waypoints[i].position, _waypoints[0].position);
                }
            }
            else
            {
                Gizmos.DrawLine(_waypoints[i].position, _waypoints[i + 1].position);
            }
        }
    }
    #endregion

    #region methods
    private void CounterClockWise()
    {
        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            _destinationID++;
            if (_destinationID > _waypoints.Length - 1)
            {
                _destinationID = 0;
            }
            _agent.SetDestination(_waypoints[_destinationID].position);
        }
    }

    private void PingPong()
    {
        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            if (_startToEnd)
            {
                _destinationID++;
            }
            else
            {
                _destinationID--;
            }
            if (_destinationID > _waypoints.Length - 1)
            {
                _startToEnd = false;
                _destinationID = _waypoints.Length - 1;
            }
            else if (_destinationID < 0)
            {
                _startToEnd = true;
                _destinationID = 0;
            }
            _agent.SetDestination(_waypoints[_destinationID].position);
        }
    }

    private void ClockWise()
    {
        if (_agent.remainingDistance < _agent.stoppingDistance)
        {
            _destinationID--;
            if (_destinationID < 0)
            {
                _destinationID = _waypoints.Length - 1;
            }
            _agent.SetDestination(_waypoints[_destinationID].position);
        }
    }

    private void DetectPlayer()
    {
        if (_coneVision._target != null)
        {
            Chasing();
            _patrolMode = PatrolType.CHASING;
        }
    }

    private void Chasing()
    {
        _agent.SetDestination(_coneVision._target.transform.position);
    }
    #endregion

    #region Private & Protected
    private NavMeshAgent _agent;
    private int _destinationID = 0;
    private bool _startToEnd = true;
    private ConeVision _coneVision;
    private PatrolType _startPatrolType;
    #endregion
}
