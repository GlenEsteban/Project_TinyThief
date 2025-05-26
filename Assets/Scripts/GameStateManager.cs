using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }

    public event Action OnPlayerCaught;

    [SerializeField] private GrabAbility _player;

    [SerializeField] private BGMPlayer _bgmPlayer;

    [SerializeField] private SceneSFX _sceneSFX;

    [SerializeField] private List<GuardAIController> _guardsGivingChase = new List<GuardAIController>();

    [SerializeField] private GameObject _capturedUI;
    [SerializeField] private GameObject _gameOverUI;


    public void AddGuardGivingChase(GuardAIController guard) {
        if (!_guardsGivingChase.Contains(guard)) {
            _guardsGivingChase.Add(guard);
        }

        ChangeGameState(GameState.ChaseSequence);
    }

    public void RemoveGuardGivingChase(GuardAIController guard) {
        if (_guardsGivingChase.Contains(guard)) {
            _guardsGivingChase.Remove(guard);
        }

        if (_guardsGivingChase.Count == 0) {
            ChangeGameState(GameState.MainGameplay);
        }
    }

    private GameState _currentGameState;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }

    private void Start() {
        RunMainGameplay();
    }

    private void Update() {
        DebugHotkeys();

        if (_guardsGivingChase.Count > 0) {
            _player.SetIsStealing(true);
        }
    }

    // TEMP: INPUT FOR TESTING GAME STATE CHANGES
    private void DebugHotkeys() {
        if (Input.GetKeyDown(KeyCode.Alpha1)) {
            ChangeGameState(GameState.MainGameplay);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            ChangeGameState(GameState.ChaseSequence);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            ChangeGameState(GameState.GameOver);
        }
    }

    public void ChangeGameState(GameState state) {
        if (_currentGameState == state) { return; }

        _currentGameState = state;

        switch (state) {
            case GameState.MainGameplay:
                RunMainGameplay();
                break;
            case GameState.ChaseSequence:
                RunChaseSequence();
                break;
            case GameState.GameOver:
                RunGameOver();
                break;
        }
    }

    private void RunMainGameplay() {
        if (_bgmPlayer == null) { return; }

        _player.SetIsStealing(false);

        _bgmPlayer.PlayBGMMain();
    }

    private void RunChaseSequence() {
        if (_bgmPlayer == null) { return; }

        _player.SetIsStealing(true);

        _bgmPlayer.PlayBGMChaseSequence();
    }

    private void RunGameOver() {
        OnPlayerCaught?.Invoke();
        _bgmPlayer.StopBGM();
        StartCoroutine(DelayBGMGameOver());
    }

    private IEnumerator DelayBGMGameOver() {
        yield return new WaitForSeconds(2);

        _capturedUI.SetActive(true);

        _sceneSFX.PlayBGMCaughtSFX();

        yield return new WaitForSeconds(1);

        _bgmPlayer.PlayBGMGameOver();

        yield return new WaitForSeconds(1);
        
        Cursor.visible = true;

        _gameOverUI.SetActive(true);
    }
}
public enum GameState {
    MainGameplay,
    ChaseSequence,
    GameOver
}
