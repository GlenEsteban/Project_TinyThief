using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class GuardAIController : MonoBehaviour {
    [SerializeField] private GrabAbility _player;
    [SerializeField] private Transform _playerCenterPoint;

    [SerializeField] private float _fovDistance = 10f;
    [SerializeField, Range(-1, 1)] private float fovRangeThreshold = 0;
    [SerializeField, Range(-1, 1)] private float _checkForCrimeRate = 0.2f;
    [SerializeField, Range(-1, 1)] private float _checkForFOVObstructionRate = 0.2f;
    [SerializeField] private Transform _FOVPoint;

    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
    [SerializeField] private float _surveyAtPatrolPointDuration = 5f;

    [SerializeField] private float _surveyingRate = 5f;
    [SerializeField] private float _surveyingRange = 5f;
    [SerializeField] private Transform _surveyingPoint;
    [SerializeField] private float _surveyingDuration = 15f;
    
    private GuardSFX _guardSFX;
    private AIMovement _aiMovement;
    private EmoteUI _emoteUI;

    public bool _isInFOVDistance = false;
    public bool _isInFOVRangeThreshold = false;
    public bool _isFOVObstructed = false;
    public bool _canSeePlayer = false;
    public bool _isInControl = true;

    public bool _isPatrolling = true;
    public bool _isSurveyingOnPatrol = true;
    public bool _hasReachedPatrolpoint = true;
    public bool _isChasing = false;
    public bool _isSurveying = false;

    private float _surveyAtPatrolPointTimer = 0;
    private float _checkForCrimeTimer = 0;
    private float _checkForFOVObstructionTimer = 0;
    private float _surveyingTimer = 0;
    private float _surveyingDurationTimer = 0;

    private Vector3 _pointLastSeenPlayer;
    private float _distanceToPlayer;
    private Vector3 _directionToPlayer;
    private Vector3 _currentPatrolPoint;

    public void DisableControls() {
        _isInControl = false;
    }

    private void Awake() {
        _guardSFX = GetComponent<GuardSFX>();
        _aiMovement = GetComponent<AIMovement>();
    }

    private void Start() {
        if (_patrolPoints.Count == 0) {
            _patrolPoints.Add(this.transform);
        } 
        else {
            _currentPatrolPoint = _patrolPoints[0].position;
        }

        _emoteUI = GetComponent<EmoteUI>();
        _emoteUI.HideEmoteUI();

        GameStateManager.Instance.OnPlayerCaught += DisableControls;
    }

    private void Update() {
        if (!_isInControl) { return; }
        if (_isChasing) {
            
            if (_distanceToPlayer < 2f) {
                _isChasing = false;
                _isInControl = false;

                GameStateManager.Instance.ChangeGameState(GameState.GameOver);
            } else {
                HandleChaseBehavior();
            }
        }
        else if (_isPatrolling) {
            HandlePatrollingBehavior();
        }

        CheckIfPlayerInFOVDistance();

        if (!_isInFOVDistance) { return; } 

        CheckIfPlayerInFOVRangeThreshold();
        CheckForFOVObstruction();

        if (_isInFOVDistance && _isInFOVRangeThreshold && !_isFOVObstructed) {
            _canSeePlayer = true;
        } else {
            if (_canSeePlayer) {
                _pointLastSeenPlayer = _player.transform.position;
                _canSeePlayer = false;
            }
        }

        if (_canSeePlayer) {
            CheckForCrime();
        }
    }

    private void CheckIfPlayerInFOVDistance() {
        _distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        if (_distanceToPlayer < _fovDistance) {
            _isInFOVDistance = true;
        }
        else {
            _isInFOVDistance = false;
        }
    }

    private void CheckIfPlayerInFOVRangeThreshold() {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;

        if (Vector3.Dot(_directionToPlayer, transform.forward) > fovRangeThreshold) {
            _isInFOVRangeThreshold = true;
        }
        else {
            _isInFOVRangeThreshold = false;
        }
    }

    private void CheckForFOVObstruction() {
        _checkForFOVObstructionTimer += Time.deltaTime;

        if (_checkForFOVObstructionTimer > _checkForFOVObstructionRate) {
            RaycastHit hit;

            _directionToPlayer = (_playerCenterPoint.position - _FOVPoint.position).normalized;
            if (Physics.Raycast(_FOVPoint.position, _directionToPlayer, out hit)) {
                if (hit.transform.gameObject == _player.gameObject) {
                    _isFOVObstructed = false;
                }
                else {
                    _isFOVObstructed = true;
                }
            }
        }
    }

    private void CheckForCrime() { 
        _checkForCrimeTimer += Time.deltaTime;

        if (_checkForCrimeTimer > _checkForCrimeRate) {
            _checkForCrimeTimer = 0;

            if (!_isChasing && _player.GetIsStealing()) {
                _isPatrolling = false;
                _emoteUI.DisplayAlertEmote();

                _guardSFX.PlayAlertSFX();

                StartCoroutine(DelayChaseSequenceStart());

                GameStateManager.Instance.AddGuardGivingChase(this);
            }
        }
    }

    private IEnumerator DelayChaseSequenceStart() {
        _aiMovement.SetTargetDestination(transform.position);
        yield return new WaitForSeconds(1f);
        _isChasing = true;
        _isPatrolling = false;
    }

    private void HandleChaseBehavior() {
        if (_canSeePlayer) {
            _emoteUI.DisplayAlertEmote();

            _aiMovement.SetTargetDestination(_player.gameObject.transform.position);

            _aiMovement.SetIsFacingPlayer(true);

            _isSurveying = false;
            _surveyingTimer = 0;
            _surveyingDurationTimer = 0;
        }
        else {
            _emoteUI.DisplayConfusedEmote();

            _isSurveying = true;

            _surveyingDurationTimer += Time.deltaTime;
            if (_surveyingDurationTimer > _surveyingDuration) {
                EndChase();
            }

            float distanceToPointLastSeenPlayer = (transform.position - _pointLastSeenPlayer).magnitude;
            if (distanceToPointLastSeenPlayer >= _surveyingRange) {
                _aiMovement.SetTargetDestination(_pointLastSeenPlayer);
            }
            else if (_isSurveying) {
                HandleSurveyingBehavior(_pointLastSeenPlayer);
            }

            _aiMovement.SetIsFacingPlayer(false);
        }
    }


    private void HandleSurveyingBehavior(Vector3 surveyingPoint) {
        _surveyingTimer += Time.deltaTime;
        if (_surveyingTimer > _surveyingRate) {
            Vector2 randomPoint = UnityEngine.Random.insideUnitCircle * _surveyingRange;
            Vector3 randomVectorXZ = new Vector3(randomPoint.x, 0f, randomPoint.y);
            _aiMovement.SetTargetDestination(surveyingPoint + randomVectorXZ);
            _surveyingTimer = 0;
        }
    }

    private void EndChase() {
        _isChasing = false;
        _isSurveying = false;
        _isPatrolling = true;

        _emoteUI.HideEmoteUI();

        GameStateManager.Instance.RemoveGuardGivingChase(this);
    }

    private void HandlePatrollingBehavior() {
        if (!_isSurveying) {
            _aiMovement.SetTargetDestination(_currentPatrolPoint);
            _hasReachedPatrolpoint = (_currentPatrolPoint - transform.position).magnitude < _surveyingRange;
        }

        if (_hasReachedPatrolpoint) {
            _isSurveying = true;

            if(_isSurveyingOnPatrol) {
                HandleSurveyingBehavior(_currentPatrolPoint);
            }

            _surveyAtPatrolPointTimer += Time.deltaTime;
            if (_surveyAtPatrolPointTimer > _surveyAtPatrolPointDuration) {
                GetNewRandomPatrolPoint();
            }
        }
    }

    private void GetNewRandomPatrolPoint() {
        Transform patrolPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
        _surveyAtPatrolPointTimer = 0;
        _currentPatrolPoint = patrolPoint.position;
        _isSurveying = false;
    }
}