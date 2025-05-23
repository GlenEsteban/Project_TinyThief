using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AIController : MonoBehaviour {
    [SerializeField] private GrabAbility _player;
    [SerializeField] private Transform _playerCenterPoint;

    [SerializeField] private float _fovDistance = 10f;
    [SerializeField, Range(-1,1)] private float fovRangeThreshold = 0;
    [SerializeField, Range(-1, 1)] private float _checkForCrimeRate= 0.2f;
    [SerializeField, Range(-1, 1)] private float _checkForFOVObstructionRate = 0.2f;
    [SerializeField] private Transform _FOVPoint;

    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();

    [SerializeField] private float _surveyingRate = 5f;
    [SerializeField] private float _surveyingRange= 5f;
    [SerializeField] private Transform _surveyingPoint;

    [SerializeField] private BGMPlayer _bgmPlayer; // NOTE: Change how you reference this!!             <<< FIX CODE


    private GuardSFX _guardSFX;
    private AIMovement _aiMovement;


    public bool _isInFOVDistance= false;
    public bool _isInFOVRangeThreshold = false;
    public bool _isFOVObstructed = false;
    public bool _canSeePlayer = false;

    public bool _isPatrolling = true;
    public bool _isChasing = false;
    public bool _isSurveying = false;


    private float _checkForCrimeTimer = 0;
    private float _checkForFOVObstructionTimer = 0;
    private float _surveyingTimer = Mathf.Infinity;
    private Vector3 _pointLastSeenPlayer;


    private Vector3 _directionToPlayer;

    private void Awake() {
        _guardSFX = GetComponent<GuardSFX>();
        _aiMovement = GetComponent<AIMovement>();
    }

    private void Start() {
        if (_patrolPoints.Count == 0)
        {
            _patrolPoints.Add(this.transform);
        }
    }

    private void Update() {
        if (_isChasing) {
            HandleChaseBehavior();
        }

        CheckIfPlayerInFOVDistance();

        if (!_isInFOVDistance) { return; }

        CheckIfPlayerInFOVRangeThreshold();

        if (!_isInFOVRangeThreshold) { return; }

        CheckForFOVObstruction();

        if (!_isFOVObstructed) { return; }

        CheckForCrime();
    }

    private void CheckIfPlayerInFOVDistance() {
        float distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        if (distanceToPlayer < _fovDistance) {
            print("in FOV Distance");
            _isInFOVDistance = true;
            _canSeePlayer = true;
        }
        else {
            _isInFOVDistance = false;
            _canSeePlayer = false;
            _isSurveying = true;

            EndChase();
        }
    }

    private void CheckIfPlayerInFOVRangeThreshold() {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;

        if (Vector3.Dot(_directionToPlayer, transform.forward) > fovRangeThreshold) {
            print("in FOV Range Threshold");
            _isInFOVRangeThreshold = true;
            _canSeePlayer = true;
        }
        else {
            _isInFOVRangeThreshold = false;
            _canSeePlayer = false;
        }
    }

    private void CheckForFOVObstruction() {
        _checkForFOVObstructionTimer += Time.deltaTime;

        if (_checkForFOVObstructionTimer > _checkForFOVObstructionRate) {
            RaycastHit hit;

            _directionToPlayer = (_playerCenterPoint.position - _FOVPoint.position).normalized;
            if (Physics.Raycast(_FOVPoint.position, _directionToPlayer, out hit)) {
                if (hit.transform.gameObject == _player.gameObject) {
                    print(hit.transform.name);
                    _isFOVObstructed = true;
                    _isSurveying = false;
                    _canSeePlayer = true;
                }
                else {
                    print(hit.transform.name);
                    _isFOVObstructed = false;
                    _canSeePlayer = false;

                    _pointLastSeenPlayer = _player.transform.position;
                }
            }
        }
    }

    private void CheckForCrime() { 
        _checkForCrimeTimer += Time.deltaTime;

        if (_checkForCrimeTimer > _checkForCrimeRate) {
            _checkForCrimeTimer = 0;

            if (!_isChasing && _player.GetIsStealing()) {
                _isChasing = true;
                _aiMovement.SetIsFacingPlayer(true);

                _surveyingTimer = Mathf.Infinity;

                _bgmPlayer.PlayBGMChaseSequence();

                _guardSFX.PlayAlertSFX();
            }
        }
    }

    private void HandleChaseBehavior() {
        if (!_isSurveying) {
            _aiMovement.SetTargetDestination(_player.gameObject.transform.position);
        }

        if (!_isInFOVDistance || !_isInFOVRangeThreshold | !_isFOVObstructed) {            
            _isSurveying = true;
            _aiMovement.SetIsFacingPlayer(false);

            _surveyingTimer += Time.deltaTime;
            if (_surveyingTimer > _surveyingRate) {
                HandleSurveyingBehavior();
                _surveyingTimer = 0;
            }
        }
    }
    Vector3 randomVectorXZ;
    private void HandleSurveyingBehavior() {
        Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * _surveyingRange;
        Vector3 randomVectorXZ = new Vector3(randomPoint.x, 0f, randomPoint.y);
        _aiMovement.SetTargetDestination(_pointLastSeenPlayer + randomVectorXZ);
    }

    private void EndChase() {
        _isChasing = false;
        _isSurveying = false;
        _isPatrolling = true;

        _bgmPlayer.PlayBGMMain();
    }

    private void ChooseRandomPatrolPoint() {
        Vector3 randomPatrolPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)].position;
        _aiMovement.SetTargetDestination(randomPatrolPoint);
    }

    private void HandlePatrollingBehavior() {

    }
}