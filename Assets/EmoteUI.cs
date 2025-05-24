using UnityEngine;
using UnityEngine.UI;

public class EmoteUI : MonoBehaviour {
    [SerializeField] private Image _EmoteUI;
    [SerializeField] private Sprite _chaseEmoteSprite;
    [SerializeField] private Sprite _surveyingEmoteSprite;

    [SerializeField] private Camera targetCamera;
    void Start() {
        if (targetCamera == null) {
            targetCamera = Camera.main;
        }
    }
    void LateUpdate() {
        if (targetCamera != null) {
            // Get direction to camera, ignoring Y-axis
            Vector3 directionToCamera = targetCamera.transform.position - transform.position;
            directionToCamera.y = 0f; // Lock Y-axis

            if (directionToCamera.sqrMagnitude > 0.001f) {
                // Apply rotation toward camera
                Quaternion lookRotation = Quaternion.LookRotation(-directionToCamera);
                _EmoteUI.transform.rotation = lookRotation;
            }
        }
    }
    public void HideEmoteUI() {
        _EmoteUI.enabled = false;
    }
    public void DisplayChaseEmote() {
        _EmoteUI.enabled = true;
        _EmoteUI.sprite = _chaseEmoteSprite;
    }

    public void DisplaySurveyingEmote() {
        _EmoteUI.enabled = true;
        _EmoteUI.sprite = _surveyingEmoteSprite;
    }
}