using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum PatrolType
{
    PINGPONG,
    CLOKWISE,
    COUNTERCLOCK
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
        _agent.Warp(_waypoints[_startingID].position);
        _agent.SetDestination(_waypoints[_startingID].position);
        _destinationID = _startingID;
    }

    void Update()
    {
        if (_coneVision._target != null && !_isChasing)
        {
            Chasing();
            _isChasing = true;
        }
        else
        {
            if (_agent.remainingDistance < _agent.stoppingDistance)
            {
               switch (_patrolMode)
                {
                    case PatrolType.CLOKWISE:
                        ClockWise();
                        break;
                    case PatrolType.COUNTERCLOCK:
                        CounterClockWise();
                        break;
                    case PatrolType.PINGPONG:
                        PingPong();
                        break;
                }
            }
        }
        if (_coneVision._target == null)
        {
            _isChasing = false;
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
        _destinationID++;
        if (_destinationID > _waypoints.Length - 1)
        {
            _destinationID = 0;
        }
        _agent.SetDestination(_waypoints[_destinationID].position);
    }

    private void PingPong()
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

    private void ClockWise()
    {
        _destinationID--;
        if (_destinationID < 0)
        {
            _destinationID = _waypoints.Length - 1;
        }
        _agent.SetDestination(_waypoints[_destinationID].position);
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
    private bool _isChasing = true;
    #endregion
}
