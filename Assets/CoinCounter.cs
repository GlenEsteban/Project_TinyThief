using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour {
    [SerializeField] private int _coinCount = 0;
    [SerializeField] private TextMeshProUGUI _coinCountTextUI;
    [SerializeField] private TextMeshProUGUI _coinsAddedTextUI;

    private int _coinsAdded = 0;
    private bool _isCoinsUpdated = true;

    public int GetCoinCount() {
        return _coinCount;
    }

    private void Start () {
        _coinCountTextUI.text = _coinCount.ToString();
    }

    public void AddToCoinCount(int coinsAdded) {
        if (!_isCoinsUpdated) {
            StopAllCoroutines();
            _coinsAdded += coinsAdded;
            _coinsAddedTextUI.text = "+" + _coinsAdded.ToString();

            _coinCount += coinsAdded;
            StartCoroutine(DelayUpdateCoinCount());
        }
        else {
            _isCoinsUpdated = false;

            _coinsAddedTextUI.gameObject.SetActive(true);

            _coinsAdded += coinsAdded;
            _coinsAddedTextUI.text = "+" + coinsAdded.ToString();

            _coinCount += coinsAdded;
            StartCoroutine(DelayUpdateCoinCount());
        }
    }

    public IEnumerator DelayUpdateCoinCount() {
        yield return new WaitForSeconds(1);
        _coinCountTextUI.text = _coinCount.ToString();
        _coinsAddedTextUI.gameObject.SetActive(false);

        _coinsAdded = 0;
        _isCoinsUpdated = true;
    }
}
