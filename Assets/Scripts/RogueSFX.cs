using UnityEngine;

public class RogueSFX : MonoBehaviour {
    [SerializeField] private AudioClip _grabSFX;
    [SerializeField] private AudioClip _movementSFX;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayGrabSFX() {
        _audioSource.pitch = Random.Range(0.8f, 1.2f);
        _audioSource.clip = _grabSFX;
        _audioSource.Play();
    }
    public void PlayMovementSFX() {
        _audioSource.pitch = 1;
        _audioSource.clip = _movementSFX;
        _audioSource.Play();
    }
}
