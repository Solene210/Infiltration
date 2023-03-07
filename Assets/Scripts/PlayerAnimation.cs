using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private PlayerController _controller;
    [SerializeField]
    [Range(0f, 1f)]
    private float _distanceToGround;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _currentSpeed = 0;
    }

    void Update()
    {
        _currentSpeed = _rb.velocity.magnitude;
        AnimationToPlay();
    }

    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if(_animator)
    //    {
    //        //left foot
    //        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1);
    //        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1);
    //        RaycastHit hit;
    //        Ray ray = new Ray(_animator.GetIKPosition(AvatarIKGoal.LeftFoot) + Vector3.up, Vector3.down);
    //        if (Physics.Raycast(ray, out hit, _distanceToGround + 1))
    //        {
    //            if (hit.transform.tag == "Walkable")
    //            {
    //                Vector3 footposition = hit.point;
    //                footposition.y += _distanceToGround;
    //                _animator.SetIKPosition(AvatarIKGoal.LeftFoot,footposition);
    //            }
    //        }
    //    }
    //}

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
            _isFalling = false;
        }
        if (_currentSpeed > 0.3f)
        {
            Vector3 localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
            _animator.SetFloat("moveSpeed", _currentSpeed);
            _animator.SetFloat("speedX", localDirection.x);
            _animator.SetFloat("speedY", localDirection.z);
            _animator.SetBool("isSneaking", false);
        }
        if (_controller.IsGrounded == false)
        {
            _isFalling = true;
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
