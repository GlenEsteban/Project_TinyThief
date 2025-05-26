using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour {
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
                transform.rotation = lookRotation;
            }
        }
    }
}
