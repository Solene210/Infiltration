using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region expose

    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _jumpForce;

    #endregion

    #region Unity Life

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        Move();
        Jump();
    }

    private void FixedUpdate()
    {
        if (_isJumping)
        {
            _direction.y = _jumpForce;
            _isJumping = false;
        }
        else
        {
            _direction.y = _rigidbody.velocity.y;
        }
        _rigidbody.velocity = _direction;
        RotateTowardsCamera();
    }
    #endregion

    #region Methods

    private void Move()
    {
        //Déplacement du joueur par rapport à la vue de la caméra
                            //Déplacement avant - arrière                           //Déplacement gauche - droite
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        _direction *= _speed;
    }

    private void RotateTowardsCamera()
    {
        if (_direction.magnitude > 0.1f)
        {
            Vector3 cameraForward = _cameraTransform.forward;
            cameraForward.y = 0;
            Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
            //Quaternion rotation = Quaternion.RotateTowards(_rigidbody.rotation, lookRotation, _turnSpeed * Time.fixedDeltaTime);
            _rigidbody.MoveRotation(lookRotation);
        }
    }

    private void Jump()
    {
        if(Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }
    }

    #endregion

    #region private & protected

    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private bool _isJumping = false;
    #endregion
}
