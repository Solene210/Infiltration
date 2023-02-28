using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    #region expose

    [Header("Movement parameter")]
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _sprintSpeed;

    [Header("Floor detection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;

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
        Collider[] groundColliders = Physics.OverlapBox(_groundChecker.position, _boxDimension, Quaternion.identity, _groundMask);
        _isGrounded = groundColliders.Length > 0;
        if (_isGrounded)
        {
            Debug.Log("Je touche le sol");
        }
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(_groundChecker.position, _boxDimension * 2f);
    }

    #endregion

    #region Methods

    private void Move()
    {
        //Déplacement du joueur par rapport à la vue de la caméra
                            //Déplacement avant - arrière                           //Déplacement gauche - droite
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        _direction *= _speed;
        if (Input.GetButton("Sprint"))
        {
            Sprint();
        }
    }

    private void Sprint()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        _direction *= _sprintSpeed;
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
        if(Input.GetButton("Jump"))
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
    private bool _isGrounded = true;

    #endregion
}
