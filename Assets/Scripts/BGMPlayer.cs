using UnityEngine;

public class BGMPlayer : MonoBehaviour {
    [SerializeField] AudioClip _bgmMain;
    [SerializeField] AudioClip _bgmChaseSequence;
    [SerializeField] AudioClip _bgmGameOver;


    private AudioSource _audioSource;    

    private void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable() {
        
    }

    private void Start() {
        PlayBGMMain();
    }

    public void PlayBGMMain() {
        _audioSource.clip = _bgmMain;
        _audioSource.Play();
    }
    public void PlayBGMChaseSequence() {
        _audioSource.clip = _bgmChaseSequence;
        _audioSource.Play();
    }
    public void PlayBGMGameOver() {
        _audioSource.loop = false;
        _audioSource.clip = _bgmGameOver;
        _audioSource.Play();
    }

    public void StopBGM() {
        _audioSource.Stop();
    }
}
