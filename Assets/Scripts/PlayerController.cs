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
    
    [Header("Slope handling")]
    [SerializeField] private float _maxSlopeAngle;

    [Header("Floor detection")]
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private Vector3 _boxDimension;
    [SerializeField] private Transform _groundChecker;
    [SerializeField] private float _yFloorOfset;
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
        //Légerement en face du joueur il y a une pente immontable
        if (SlopeAngle() > _maxSlopeAngle)
        {
            Debug.Log("Ne doit pas avancer");   // Mais quand même déjà un peu sur la pente
            Vector3 localDirection = transform.InverseTransformDirection(_direction);   //Passe du gloabal au local
            if(localDirection.z > 0) localDirection.z = 0;   //Pas le droit d'avancer
            _direction = transform.TransformDirection(localDirection);  //Repasse la direction en global
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
        Direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        Direction *= _speed;
        _direction.y = 0; //on veut pas bouger en altidute par rapport a la camera
        if (Input.GetButton("Sprint"))
        {
            Sprint();
        }
        if (Input.GetButton("Fire1"))
        {
            Sneaking();
        }
    }

    private void Sprint()
    {
        Direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        Direction *= _sprintSpeed;
        _direction.y = 0; //on veut pas bouger en altidute par rapport a la camera
    }

    public void Sneaking()
    {
        Direction = _cameraTransform.forward * Input.GetAxis("Vertical") + _cameraTransform.right * Input.GetAxis("Horizontal");
        Direction *= _sneakingSpeed;
        _direction.y = 0; //on veut pas bouger en altidute par rapport a la camera
    }

    private void StickToGround()
    {
        Vector3 averagePosition = _floorDetector.AverageHeight();
        Vector3 newPosition = new Vector3(_rigidbody.position.x, averagePosition.y + _yFloorOfset, _rigidbody.position.z);
        _rigidbody.MovePosition(newPosition);
    }

    private void RotateTowardsCamera()
    {
        if (Direction.magnitude > 0.1f)
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

    private float SlopeAngle()
    {
        Vector3 startingpoint = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.5f);
            Debug.DrawRay(startingpoint, Vector3.down);
        if (Physics.Raycast(startingpoint, Vector3.down, out _slopeHit))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle;
        }
        return 370;
    }
    #endregion

    public bool IsJumping { get => _isJumping; set => _isJumping = value; }
    public bool IsGrounded { get => _isGrounded; set => _isGrounded = value; }
    public Vector3 Direction { get => _direction; set => _direction = value; }

    #region private & protected
    private Vector3 _direction = new Vector3();
    private Rigidbody _rigidbody;
    private Transform _cameraTransform;
    private FloorDetector _floorDetector;
    public bool _isJumping = false;
    public bool _isGrounded = true;
    private RaycastHit _slopeHit;
    #endregion
}
