using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VIllagerAIController : MonoBehaviour {
    [SerializeField] private List<Transform> _patrolPoints = new List<Transform>();
    [SerializeField] private float _surveyAtPatrolPointDuration = 5f;

    [SerializeField] private float _surveyingRate = 5f;
    [SerializeField] private float _surveyingRange = 5f;
    [SerializeField] private Transform _surveyingPoint;
    [SerializeField] private float _surveyingDuration = 15f;

    public bool _isSurveying;
    public Vector3 _currentPatrolPoint;
    private AIMovement _aiMovement;
    public bool _hasReachedPatrolpoint;
    private float _surveyingTimer;
    private float _surveyAtPatrolPointTimer;

    private void Awake() {
        _aiMovement = GetComponent<AIMovement>();
    }

    private void Start() {
        if (_patrolPoints.Count == 0) {
            _patrolPoints.Add(this.transform);
        }
        else {
            _currentPatrolPoint = _patrolPoints[0].position;
        }
    }

    private void Update() {
        HandlePatrollingBehavior();
    }

    private void HandlePatrollingBehavior() {
        if (!_isSurveying) {
            _aiMovement.SetTargetDestination(_currentPatrolPoint);
            _hasReachedPatrolpoint = (_currentPatrolPoint - transform.position).magnitude < _surveyingRange;
        }

        if (_hasReachedPatrolpoint) {
            _isSurveying = true;

            HandleSurveyingBehavior(_currentPatrolPoint);

            _surveyAtPatrolPointTimer += Time.deltaTime;
            if (_surveyAtPatrolPointTimer > _surveyAtPatrolPointDuration) {
                GetNewRandomPatrolPoint();
            }
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

    private void GetNewRandomPatrolPoint() {
        Transform patrolPoint = _patrolPoints[UnityEngine.Random.Range(0, _patrolPoints.Count)];
        _surveyAtPatrolPointTimer = 0;
        _currentPatrolPoint = patrolPoint.position;
        _isSurveying = false;
    }
}
