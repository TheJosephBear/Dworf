using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDui : UIBehaviour {
    public static HUDui Instance { get; private set; }

    public TextMeshProUGUI scoreOne;
    public TextMeshProUGUI scoreTwo;
    public TextMeshProUGUI endScoreHigh;
    public TextMeshProUGUI endScoreYou;

    public Slider p1BaseSlider;
    public Slider p1SuperSlider;
    public Slider p2BaseSlider;
    public Slider p2SuperSlider;

    public GameObject gameOverScreen;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            gameOverScreen.SetActive(false);
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

    public void SetScoreOne(int score) {
        scoreOne.text = score.ToString();
    }
    public void SetScoreTwo(int score) {
        scoreTwo.text = score.ToString();
    }

    public void ToggleGameOver(bool on) {
        gameOverScreen.SetActive(on);
    }

    public void ShowEndScore(int high, int you) {
        endScoreHigh.text = high.ToString();
        endScoreYou.text = you.ToString();
    }

}
