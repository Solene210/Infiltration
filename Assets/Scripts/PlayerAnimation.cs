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
        if (_currentSpeed < 0.5f)
        {
            _currentSpeed = 0;
            _localDirection.x = 0;
            _localDirection.z = 0;
        }
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
        _localDirection = transform.InverseTransformDirection(_controller.Direction);   //Passe du gloabal au local
        _animator.SetBool("isJumping", _controller.IsJumping);
        _animator.SetBool("isGrounded", _controller.IsGrounded);
        _animator.SetFloat("moveSpeed", _currentSpeed);
        _animator.SetFloat("speedX", _localDirection.x);
        _animator.SetFloat("speedY", _localDirection.z);
        _animator.SetBool("isSneaking", _controller.IsSneaking);
    }

    private Vector3 _localDirection;
    private float _currentSpeed;
    private Rigidbody _rb;
}
