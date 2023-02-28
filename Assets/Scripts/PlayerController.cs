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
    [SerializeField] private float _sneakingSpeed;
    [SerializeField] private float _maxSlopeAngle;

    [Header("Floor detection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _yFloorOfset;
    [SerializeField] private float _playerHeight;

    #endregion

    #region Unity Life

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        _floorDetector = GetComponentInChildren<FloorDetector>();
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
    }

    private void FixedUpdate()
    {
        if (_isGrounded)
        {
            StickToGround();
          
            if (_isJumping)
            {
                _isGrounded = false;
                _direction.y = _jumpForce;
                _isJumping = false;
            }
        }
        else
        {
            //Ici soit on saute soit on tombe
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
        _direction.y = 0; //on veut pas bouger en altidute par rapport a la camera
        if (Input.GetButton("Sprint"))
        {
            Sprint();
        }
        if (Input.GetButton("Fire1"))
        {
            Sneaking();
        }
        //if(OnSlope())
        //{
        //    _rigidbody.AddForce(GetSlopeMoveDirection() * _speed * 20, ForceMode.Force);
        //}
    }

    private void Sprint()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        _direction *= _sprintSpeed;
    }

    private void Sneaking()
    {
        _direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        _direction *= _sneakingSpeed;
    }

    private void StickToGround()
    {
        Vector3 averagePosition = _floorDetector.AverageHeight();
        Vector3 newPosition = new Vector3(_rigidbody.position.x, averagePosition.y + _yFloorOfset, _rigidbody.position.z);
        //transform.position = newPosition;
        _rigidbody.MovePosition(newPosition);

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
        if (Input.GetButtonDown("Jump"))
        {
            _isJumping = true;
        }
    }

    //private bool OnSlope()
    //{
    //    if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, _playerHeight * 0.5f + 0.3f))
    //    {
    //        float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
    //        return angle < _maxSlopeAngle && angle != 0;
    //    }
    //    return false;
    //}

    //private Vector3 GetSlopeMoveDirection()
    //{
    //    return Vector3.ProjectOnPlane(_direction, slopeHit.normal).normalized;
    //}

    #endregion

    #region private & protected

    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private FloorDetector _floorDetector;
    public bool _isJumping = false;
    public bool _isGrounded = true;
    private RaycastHit slopeHit;

    #endregion
}
