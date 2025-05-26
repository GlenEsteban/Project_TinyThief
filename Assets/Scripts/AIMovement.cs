using System;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class AIMovement : MonoBehaviour {
    [SerializeField] private bool _isAbleToMove = true;
    [SerializeField] private Vector3 _targetDestination;
    [SerializeField] private Transform _player;
    [SerializeField, Range(0, 100)] private float _moveSpeed = 10f;
    [SerializeField, Range(0, 100)] private float _moveRotationSpeed = 10f;
    [SerializeField] private bool _isFacingPlayer = false;

    public void SetIsFacingPlayer(bool state) {
        _isFacingPlayer = state;
    }

    private NavMeshAgent _navMeshAgent;
    private Animator _animator;

    private float _movementParameterValue;

    public void SetTargetDestination(Vector3 position) {
        _targetDestination = position;
    }

    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
    }

    private void Start() {
        _targetDestination = transform.position;
        _navMeshAgent.speed = _moveSpeed;
    }

    private void Update() { 
        if (!_isAbleToMove) { return; }

        _navMeshAgent.SetDestination(_targetDestination);

        _movementParameterValue = _navMeshAgent.velocity.magnitude / _navMeshAgent.speed;
        _animator.SetFloat("Movement", _movementParameterValue);

        if (_navMeshAgent.velocity.magnitude > 0) {
            Vector3 targetDirection;
            if (_isFacingPlayer) {
                targetDirection = (_player.position - transform.position).normalized;

            }
            else {
                targetDirection = _navMeshAgent.velocity.normalized;
            }
        }
    }
}
