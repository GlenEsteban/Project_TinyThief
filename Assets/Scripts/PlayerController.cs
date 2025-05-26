using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    private PlayerInput _playerInput;
    private Movement _movement;
    private GrabAbility _grabAbility;
    private EmoteUI _emoteUI;

    private Vector2 _moveDirection;
    private Vector2 _lookDirection;

    private bool _isMoving;
    private bool _isLookingAround;
    private bool _isPlayerInControl = true;

    public void HandleDisableControlTemporarily() {
        StartCoroutine(DisableControlTemporarily(0.3f));

        _moveDirection = Vector2.zero;
        _movement.SetMoveDirection(_moveDirection);
    }

    private void OnEnable() {
        _playerInput.Enable();

        _playerInput.Player.Move.performed += Move;
        _playerInput.Player.Move.canceled += Move;
        _playerInput.Player.Look.performed += Look;
        _playerInput.Player.Look.canceled += Look;
        _playerInput.Player.Interact.performed += Interact;
        _playerInput.Player.Interact.canceled += Interact;
    }
    private void OnDisable() {
        _playerInput.Disable();

        _playerInput.Player.Move.performed -= Move;
        _playerInput.Player.Move.canceled -= Move;
        _playerInput.Player.Look.performed -= Look;
        _playerInput.Player.Look.canceled -= Look;
        _playerInput.Player.Interact.performed -= Interact;
        _playerInput.Player.Interact.canceled -= Interact;
    }

    private void Awake() {
        _playerInput = new PlayerInput();
        _movement = GetComponent<Movement>();
        _grabAbility = GetComponent<GrabAbility>();
        _emoteUI = GetComponent<EmoteUI>();
    }

    private void Start() {
        GameStateManager.Instance.OnPlayerCaught += HandlePlayerCaught;

        _grabAbility.OnGrabItem += HandleDisableControlTemporarily;
    }

    private void Update() {
        if (!_isPlayerInControl) { return; }

        if (_isMoving) {
            _movement.SetMoveDirection(_moveDirection);
        }

        if (_isLookingAround) {
            _movement.RotateFollowCam(_lookDirection);
        }
    }

    private void Move(InputAction.CallbackContext context) {
        if (!_isPlayerInControl) { return; }

        if (context.phase == InputActionPhase.Performed) {
            _isMoving = true;

            _moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled) {
            _isMoving = false;

            _moveDirection = Vector3.zero;
            _movement.SetMoveDirection(_moveDirection);
        }
    }

    private void Look(InputAction.CallbackContext context) {
        if (!_isPlayerInControl) { return; }

        if (context.phase == InputActionPhase.Performed) {
            _isLookingAround = true;
            _lookDirection = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled) {
            _isLookingAround = false;
            _lookDirection = Vector3.zero;
        }
    }

    private void Interact(InputAction.CallbackContext context) {
        if (!_isPlayerInControl) { return; }

        if (context.phase == InputActionPhase.Performed) {
            _grabAbility.Grab();
        }
    }
    private IEnumerator DisableControlTemporarily(float seconds) {
        _isPlayerInControl = false;
        yield return new WaitForSeconds(seconds);
        _isPlayerInControl = true;
    }

    private void HandlePlayerCaught() {
        DisableControls();

        _emoteUI.DisplayAlertEmote();
    }

    private void DisableControls() {
        _isPlayerInControl = false;
        _movement.StopMovement();
    }
}
