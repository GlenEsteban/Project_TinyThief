using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrespassZone : MonoBehaviour {
    private GrabAbility _player;

    private bool _isInTriggerZone = false;
    private void OnTriggerEnter(Collider other) {
        var obj = other.gameObject.GetComponent<GrabAbility>();
        if (obj != null) {
            _isInTriggerZone = true;
            _player = obj;
        }
    }
    private void OnTriggerExit(Collider other) {
        var obj = other.gameObject.GetComponent<GrabAbility>();
        if (obj != null) {
            _isInTriggerZone = false;
            obj.SetIsStealing(false);
        }
    }

    private void Update() {
        if (_isInTriggerZone) {
            _player.SetIsStealing(true);
        }
    }
}
