using UnityEngine;

public class GuardSFX : MonoBehaviour {
    [SerializeField] private AudioClip _alertSFX;
    [SerializeField] private AudioClip _movementSFX;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayAlertSFX() {
        if (_audioSource.isPlaying) { return; }
        _audioSource.clip = _alertSFX;
        _audioSource.Play();
    }
    public void PlayMovementSFX() {
        _audioSource.clip = _movementSFX;
        _audioSource.Play();
    }
}
