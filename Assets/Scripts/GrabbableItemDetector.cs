using System.Collections.Generic;
using UnityEngine;

public class GrabbableItemDetector : MonoBehaviour {
    [SerializeField] private List<GrabbableItem> _items = new List<GrabbableItem>();

    public void RemoveItem(GrabbableItem item) {
        if (_items.Contains(item)) {
            _items.Remove(item);
        }
    }

    public GrabbableItem GetFirstGrabbableItem() {
        if (_items.Count == 0) return null;
        return _items[0];
    }

    private void OnTriggerEnter(Collider other) {
        var item = other.GetComponent<GrabbableItem>();
        if (item != null) {
            _items.Add(item);

            item.ShowGrabIndicator();
        }
    }

    private void OnTriggerExit(Collider other) {
        var item = other.GetComponent<GrabbableItem>();
        if (item != null && _items.Contains(item)) {
            _items.Remove(item);

            item.HideGrabIndicator();
        }
    }
}
