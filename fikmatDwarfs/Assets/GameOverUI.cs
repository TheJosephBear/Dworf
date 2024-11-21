using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverUI : UIBehaviour {

    public static GameOverUI Instance { get; private set; }

    public TextMeshProUGUI endScoreHigh;
    public TextMeshProUGUI endScoreYou;


    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public override void Hide() {
        canvas.SetActive(false);
    }

    public override void Show() {
        canvas.SetActive(true);
    }

    public void ShowEndScore(int high, int you) {
        endScoreHigh.text = high.ToString();
        endScoreYou.text = you.ToString();
    }
}
