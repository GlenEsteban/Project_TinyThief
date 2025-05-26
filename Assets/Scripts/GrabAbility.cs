using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class GrabAbility : MonoBehaviour {
    public event Action OnGrabItem;

    [SerializeField] GrabbableItemDetector _pickupGroundDetector;
    [SerializeField] GrabbableItemDetector _pickupFrontDetector;
    [SerializeField] Transform _inventory;

    private Animation _animation;
    private RogueSFX _rogueSFX;

    [SerializeField] bool _isStealing = false;

    public bool GetIsStealing() {
        return _isStealing;
    }

    private void Awake() {
        _animation = GetComponent<Animation>();
        _rogueSFX = GetComponent<RogueSFX>();
    }

    public void Grab() {
        if (_pickupFrontDetector.GetFirstGrabbableItem() != null) {
            GrabbableItem item = _pickupFrontDetector.GetFirstGrabbableItem();
            AddToInventory(item);

            _pickupFrontDetector.RemoveItem(item);
            _pickupGroundDetector.RemoveItem(item);


            _animation.AnimateInteract();
        }
        else if (_pickupGroundDetector.GetFirstGrabbableItem() != null) {
            GrabbableItem item = _pickupGroundDetector.GetFirstGrabbableItem();
            AddToInventory(item);

            _pickupFrontDetector.RemoveItem(item);
            _pickupGroundDetector.RemoveItem(item);

            _animation.AnimatePickupItem();
        }
    }

    public void AddToInventory(GrabbableItem item) {
        OnGrabItem?.Invoke();

        _rogueSFX.PlayGrabSFX();

        HandleStealingState(item);

        InventoryManager.Instance.AddItem(item);
        item.transform.SetParent(_inventory);
        item.HideItem();
    }

    private void HandleStealingState(GrabbableItem item) {
        if (item.GetIsOwned()) {
            StartCoroutine(EnableStealingStatusTemporarily());
        }
    }

    private IEnumerator EnableStealingStatusTemporarily() {
        _isStealing = true;
        yield return new WaitForSeconds(.5f);
        _isStealing = false;
    }
}