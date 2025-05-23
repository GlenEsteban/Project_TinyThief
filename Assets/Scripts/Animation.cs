using UnityEngine;

public class Animation : MonoBehaviour {
    private Animator _animator;

    private void Awake() {
        _animator = GetComponent<Animator>();
    }

    public void AnimateJump() {
        _animator.SetTrigger("Jump");
    }

    public void AnimateInteract() {
        _animator.SetTrigger("Interact");
    }

    public void AnimateCheer() {
        _animator.SetTrigger("Cheer");
    }

    public void AnimatePickupItem() {
        _animator.SetTrigger("PickupItem");
    }
    public void AnimateRaiseItem() {
        _animator.SetTrigger("RaiseItem");
    }
    public void AnimateUseItem() {
        _animator.SetTrigger("UseItem");
    }
}
