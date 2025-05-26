using UnityEngine;

public class SceneSFX : MonoBehaviour {
    [SerializeField] AudioClip _caughtSFX;

    private AudioSource _audioSource;

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }
    public void PlayBGMCaughtSFX() {
        _audioSource.clip = _caughtSFX;
        _audioSource.Play();
    }
}
