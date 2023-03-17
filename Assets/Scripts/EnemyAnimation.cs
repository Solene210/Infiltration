using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAnimation : MonoBehaviour
{
    #region Expose
    [SerializeField] private Animator _animator;
    #endregion

    #region Unity Life Cycle
    private void Awake()
    {
        
    }

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _isGrounded = true;
    }

    void Update()
    {
        _currentSpeed = _agent.acceleration;
        AnimationToPlay();
    }
    #endregion

    #region methods
    private void AnimationToPlay()
    {
        _localDirection = transform.InverseTransformDirection(transform.position);   //Passe du gloabal au local
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("moveSpeed", _currentSpeed);
        _animator.SetFloat("speedX", transform.position.x);
        _animator.SetFloat("speedY", transform.position.z);
    }

    #endregion

    #region Private & Protected
    private float _currentSpeed;
    private bool _isGrounded;
    private NavMeshAgent _agent;
    private Vector3 _localDirection;
    #endregion
}
