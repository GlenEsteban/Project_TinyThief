using UnityEngine;

public class GrabbableItem: MonoBehaviour{
    [SerializeField] private string _name;
    [SerializeField] private int _value;
    [SerializeField] private bool _isOwned = false;
    [SerializeField] private GameObject _mesh;



    public string GetName() {
        return _name;
    }
    public int GetValue() {
        return _value;
    }

    public bool GetIsOwned() {
        return _isOwned;
    }

    public void DisownItem() {
        _isOwned = false;
    }

    public void HideItem() {
        gameObject.SetActive(false);
    }
}