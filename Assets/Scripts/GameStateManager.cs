using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }

    public event Action OnPlayerCaught;

    [SerializeField] private BGMPlayer _bgmPlayer;

    [SerializeField] private SceneSFX _sceneSFX;

    [SerializeField] private List<AIController> _guardsGivingChase = new List<AIController>();

    [SerializeField] private GameObject _capturedUI;

    public void AddGuardGivingChase(AIController guard) {
        if (!_guardsGivingChase.Contains(guard)) {
            _guardsGivingChase.Add(guard);
        }

        ChangeGameState(GameState.ChaseSequence);
    }

    public void RemoveGuardGivingChase(AIController guard) {
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

        _bgmPlayer.PlayBGMMain();
    }

    private void RunChaseSequence() {
        if (_bgmPlayer == null) { return; }

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
    }
}
public enum GameState {
    MainGameplay,
    ChaseSequence,
    GameOver
}
