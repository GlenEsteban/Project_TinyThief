using UnityEngine;

public class GuardBehavior : MonoBehaviour {
    [SerializeField] private GrabAbility _player;
    [SerializeField] private Transform _playerCenterPoint;

    [SerializeField] private float _fovDistance = 10f;
    [SerializeField, Range(-1,1)] private float fovRangeThreshold = 0;
    [SerializeField, Range(-1, 1)] private float _checkForCrimeRate= 0.2f;
    [SerializeField] private Transform _lookingPoint;

    private GuardSFX _guardSFX;

    public bool _canSeePlayer = false;
    public bool _isAlert = false;

    private float _checkForCrimeTimer = 0;
    private Vector3 _directionToPlayer;

    private void Awake() {
        _guardSFX = GetComponent<GuardSFX>();
    }

    private void Update() {

        CheckIfPlayerInFOVDistance();

        if (!_canSeePlayer) { return; }

        CheckIfPlayerInFOVRangeThreshold();

        if (!_canSeePlayer) { return; }

        CheckIfPlayerBehindObjects();

        if (!_canSeePlayer) { return; }

        CheckForCrime();
    }

    private void CheckIfPlayerInFOVDistance() {
        float distanceToPlayer = (_player.transform.position - transform.position).magnitude;

        if (distanceToPlayer < _fovDistance) {
            print("in FOV Distance");
            _canSeePlayer = true;
        }
        else {
            _canSeePlayer = false;
        }
    }

    private void CheckIfPlayerInFOVRangeThreshold() {
        _directionToPlayer = (_player.transform.position - transform.position).normalized;

        if (Vector3.Dot(_directionToPlayer, transform.forward) > fovRangeThreshold) {
            print("in FOV Range Threshold");
            _canSeePlayer = true;
        }
        else {
            _canSeePlayer = false;
        }
    }

    private void CheckIfPlayerBehindObjects() {
        RaycastHit hit;
        _directionToPlayer = (_playerCenterPoint.position - _lookingPoint.position).normalized;
        if (Physics.Raycast(_lookingPoint.position, _directionToPlayer, out hit)) {
            if (hit.transform.gameObject == _player.gameObject) {
                print(hit.transform.name);
                _canSeePlayer = true;
            }
            else {
                print(hit.transform.name);
                _canSeePlayer = false;
            }
        }
    }

    private void CheckForCrime() { 
        _checkForCrimeTimer += Time.deltaTime;

        if (_checkForCrimeTimer > _checkForCrimeRate) {
            _checkForCrimeTimer = 0;

            if (!_isAlert && _player.GetIsStealing()) {
                _isAlert = true;

                print("ALERT!!");

                _guardSFX.PlayAlertSFX();
            }
        }
    }
}
