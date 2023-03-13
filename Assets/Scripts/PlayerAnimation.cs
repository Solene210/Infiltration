using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController _controller;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentSpeed = 0;
    }

    void Update()
    {
        _currentSpeed = _rb.velocity.magnitude;
        if (_currentSpeed < 0.5f)
        {
            IsIdle = true;
            _currentSpeed = 0;
            _localDirection.x = 0;
            _localDirection.z = 0;
        }
        else
        {
            IsIdle = false;
        }
        AnimationToPlay();
    }
    
    private void AnimationToPlay()
    {
        _localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
        _animator.SetBool("isJumping", _controller.IsJumping);
        _animator.SetBool("isGrounded", _controller.IsGrounded);
        _animator.SetFloat("moveSpeed", _currentSpeed);
        _animator.SetFloat("speedX", _localDirection.x);
        _animator.SetFloat("speedY", _localDirection.z);
        _animator.SetBool("isSneaking", _controller.IsSneaking);
    }

    public bool IsIdle { get => _isIdle; set => _isIdle = value; }

    private Vector3 _localDirection;
    private Rigidbody _rb;
    private float _currentSpeed;
    private bool _isIdle = true;
}
