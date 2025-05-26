using UnityEngine;
using UnityEngine.UI;

public class EmoteUI : MonoBehaviour {
    [SerializeField] private Image _EmoteUIImage;
    [SerializeField] private Sprite _alertEmoteSprite;
    [SerializeField] private Sprite _confusedEmoteSprite;

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
                _EmoteUIImage.transform.rotation = lookRotation;
            }
        }
    }
    public void HideEmoteUI() {
        _EmoteUIImage.enabled = false;
    }
    public void DisplayAlertEmote() {
        _EmoteUIImage.enabled = true;
        _EmoteUIImage.sprite = _alertEmoteSprite;
    }

    public void DisplayConfusedEmote() {
        _EmoteUIImage.enabled = true;
        _EmoteUIImage.sprite = _confusedEmoteSprite;
    }
}