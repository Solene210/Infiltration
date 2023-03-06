using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PlayerController _controller;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        _currentSpeed = _rb.velocity.magnitude;
        //Vector3 localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
        AnimationToPlay();
    }

    private void AnimationToPlay()
    {
        if (_controller.IsJumping == true)
        {
            _animator.SetBool("isJumping", true);
            //_animator.SetBool("isGrounded", false);
        }
        if (_controller.IsGrounded == true)
        {
            //_animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", true);
        }
        if (_currentSpeed > 0.3f)
        {
            Vector3 localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
            _animator.SetFloat("moveSpeed", _currentSpeed);
            _animator.SetFloat("speedX", localDirection.x);
            _animator.SetFloat("speedY", localDirection.z);
        }
        if (_isFalling == true)
        {
            _animator.SetBool("isJumping", false);
            _animator.SetBool("isGrounded", false);
        }
    }

    private float _currentSpeed;
    private Rigidbody _rb;
    private bool _isFalling;

}
