using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour {
    [SerializeField, Range(0, 100)] private float _moveSpeed = 10f;
    [SerializeField, Range(0, 100)] private float _moveRotationSpeed = 10f;
    [SerializeField, Range(0, 100)] private float _lookRotationSpeed = 10f;
    [SerializeField] private Vector3 _localGravityScale = new Vector3(0, -20f, 0);
    [SerializeField] private Transform _followCamera;


    private Rigidbody _rigidBody;
    private Vector3 _moveDirection;
    private Vector3 _moveVelocity;

    private Animator _animator;
    private float _movementParameterValue;

    private bool _isAbleToMove = true;

    public void SetMoveDirection(Vector2 direction) {
        _moveDirection = new Vector3(direction.x, 0, direction.y);
        _moveDirection = _followCamera.TransformDirection(_moveDirection);
    }

    public void SetLookRotationSpeed(float speed) {
        _lookRotationSpeed = speed;
    }

    public void Start() {
        _rigidBody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }
    public void Update() {
        if (!_isAbleToMove) { return; }

        _rigidBody.AddForce(_localGravityScale, ForceMode.Acceleration);

        _moveVelocity.x = _moveDirection.x * _moveSpeed;
        _moveVelocity.z = _moveDirection.z * _moveSpeed;
        _moveVelocity.y = _rigidBody.velocity.y;
        _rigidBody.velocity = _moveVelocity;

        if (_moveDirection != Vector3.zero) {
            Quaternion toRotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _moveRotationSpeed * Time.deltaTime);
        }
        else {
            _rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, Vector3.zero,Time.deltaTime);
        }
    }

    public void LateUpdate() {
        _movementParameterValue = _rigidBody.velocity.magnitude / _moveSpeed;
        _animator.SetFloat("Movement", _movementParameterValue);
    }

    public void RotateFollowCam(Vector2 direction) {
        float yRotation = direction.x * _lookRotationSpeed * Time.deltaTime;
        _followCamera.Rotate(0f, yRotation, 0f);
    }

    public void StopMovement() {
        _rigidBody.velocity = Vector3.zero;
        _isAbleToMove = false;
    }
}