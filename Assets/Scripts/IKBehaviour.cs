using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKBehaviour : MonoBehaviour
{
    #region Expose
    [SerializeField][Range(0, 1)] private float _distanceToGround;
    [SerializeField][Range(0, 0.5f)] private float _startRaycast;
    [SerializeField] private Transform _headPosition;
    public PlayerAnimation _playerAnimation;
    #endregion

    #region Unity Life Cycle
    private void Start()
    {
        TryGetComponent<Animator>(out _animator);
    }
    private void OnAnimatorIK(int layerIndex)
    {
        //head
        _animator.SetLookAtWeight(0.35f);
        _animator.SetLookAtPosition(_headPosition.position);
        //left foot && right foot
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat("LeftFoot"));
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, _animator.GetFloat("LeftFoot"));
        RaycastHit hitL;
        Vector3 leftLegStartPosition = _animator.GetBoneTransform(HumanBodyBones.LeftLowerLeg).position;
        Vector3 leftlegStartPositionLocal = transform.InverseTransformDirection(leftLegStartPosition);
        leftlegStartPositionLocal.x -= _startRaycast;
        leftLegStartPosition = transform.TransformDirection(leftlegStartPositionLocal);
        Ray rayL = new Ray(leftLegStartPosition, Vector3.down);
        if (Physics.Raycast(rayL, out hitL, _distanceToGround + 1))
        {
            Debug.DrawRay(rayL.origin,Vector3.down, Color.red);
            if (hitL.transform.tag == "Walkable")
            {
                Vector3 footpositionL = hitL.point;
                Quaternion footRotationL = Quaternion.FromToRotation(Vector3.up, hitL.normal) * Quaternion.LookRotation(transform.forward);
                footpositionL.y += _distanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.LeftFoot, footpositionL);
                _animator.SetIKRotation(AvatarIKGoal.LeftFoot, footRotationL);
            }
        }
        _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, _animator.GetFloat("RightFoot"));
        _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, _animator.GetFloat("RightFoot"));
        RaycastHit hitR;
        Ray rayR = new Ray(_animator.GetBoneTransform(HumanBodyBones.RightUpperLeg).position, Vector3.down);
        if (Physics.Raycast(rayR, out hitR, _distanceToGround + 1))
        {
            if (hitR.transform.tag == "Walkable")
            {
                Vector3 footpositionR = hitR.point;
                Quaternion footRotationR = Quaternion.FromToRotation(Vector3.up, hitL.normal) * Quaternion.LookRotation(transform.forward);
                footpositionR.y += _distanceToGround;
                _animator.SetIKPosition(AvatarIKGoal.RightFoot, footpositionR);
                _animator.SetIKRotation(AvatarIKGoal.RightFoot, footRotationR);
            }
        }
    }
    #endregion

    #region Private & Protected
    private Animator _animator;
    #endregion
}
