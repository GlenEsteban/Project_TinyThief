using UnityEngine;

public class RogueSFX : MonoBehaviour {
    [SerializeField] private AudioClip _grabSFX;
    [SerializeField] private AudioClip _movementSFX;
    [SerializeField] private AudioClip _caughtSFX;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        GameStateManager.Instance.OnPlayerCaught += PlayCaughtSFX;
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
    public void PlayCaughtSFX() {
        _audioSource.pitch =  1.2f;
        _audioSource.PlayOneShot(_caughtSFX);
    }
}
