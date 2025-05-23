using UnityEngine;

public class RogueSFX : MonoBehaviour {
    [SerializeField] private AudioClip _grabSFX;
    [SerializeField] private AudioClip _movementSFX;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGrabSFX() {
        _audioSource.clip = _grabSFX;
        _audioSource.Play();
    }
    public void PlayMovementSFX() {
        _audioSource.clip = _movementSFX;
        _audioSource.Play();
    }
}
