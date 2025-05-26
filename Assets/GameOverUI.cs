using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _coinCountTextUI;
    [SerializeField] private CoinCounter _coinCounter;
    [SerializeField] private Button _playAgainButton;

    private void Start () {
        _coinCountTextUI.text = _coinCounter.GetCoinCount().ToString();

        StartCoroutine(DelayDisplayRetryButton());
    }

    private IEnumerator DelayDisplayRetryButton() {
        yield return new WaitForSeconds(5f);

        //
    }
}
