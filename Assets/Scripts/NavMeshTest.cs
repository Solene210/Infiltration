using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTest : MonoBehaviour
{
    #region Expose
    [SerializeField] private Transform _target;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                _agent.SetDestination(hit.point);
            }
        }
        if (_target != null)
        {
            _agent.SetDestination(_target.position);
            if (_agent.isStopped && _agent.remainingDistance > 1)
            {
                Debug.Log("je ne peux pas atteindre la cible");
            }
        }
    }
    #endregion

    #region Private & Protected
    private NavMeshAgent _agent;
    #endregion
}
