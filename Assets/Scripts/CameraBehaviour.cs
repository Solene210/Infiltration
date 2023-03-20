using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _rightLimit;
    [SerializeField] private Transform _leftLimit;
    [SerializeField] private LayerMask _rayLayer;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private IntVariable _cameraSee;
    [SerializeField] private int _score;

    void Start()
    {
        _target = _rightLimit;
    }

    void Update()
    {
        if (_playerTransform != null)
        {
            transform.LookAt(_playerTransform.position);
            if(_resetTimer < Time.timeSinceLevelLoad)
            {
                _playerTransform = null;
            }
        }
        else
        {
            PingPong();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Vector3 rayDirection = other.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, rayDirection, out hit, Mathf.Infinity, _rayLayer))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.Log("Je t'observe");
                    _playerTransform = other.transform;
                    _cameraSee.m_value += _score;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _resetTimer = Time.timeSinceLevelLoad + 2;
        }
    }

    private void PingPong()
    {
        Vector3 lookPosition = Vector3.RotateTowards(transform.forward, _target.localPosition, _rotateSpeed * Time.deltaTime, 0);
        Quaternion lookRotation = Quaternion.LookRotation(lookPosition);
        transform.rotation = lookRotation;
        Quaternion targetRotation = Quaternion.LookRotation(_target.localPosition);
        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.5f)
        {
            _rightToLeft = !_rightToLeft;
            if (_rightToLeft)
            {
                _target = _rightLimit;
            }
            else
            {
                _target = _leftLimit;
            }
        }
    }

    private bool _rightToLeft = true;
    private Transform _target;
    private Transform _playerTransform;
    private float _resetTimer;
}
