using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour {
    public static InventoryManager Instance;
    [SerializeField] private int _goldCount = 0;
    [SerializeField] private List<GrabbableItem> _inventory = new List<GrabbableItem>();

    public void AddItem(GrabbableItem item) {
        _inventory.Add(item);
    }

    public void RemoveItem(GrabbableItem item) {
        if (_inventory.Contains(item)) {
            _inventory.Remove(item);
        }
    }

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        }
        else {
            Instance = this;
        }
    }
}
