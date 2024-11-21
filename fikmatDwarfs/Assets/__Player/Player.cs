using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool isPlayerOne = true;
    public int score = 0;
    public bool isReady = false;
    public bool isAlive = true;
    public PlayerCharacter PlayerCharacter; // prefab
    Slider basePowerSlider;
    Slider specialPowerSlider;

    private void OnEnable() {
        if(isPlayerOne) {
            basePowerSlider = HUDui.Instance?.p1BaseSlider;
            specialPowerSlider = HUDui.Instance?.p1SuperSlider;
        } else {
            basePowerSlider = HUDui.Instance?.p2BaseSlider;
            specialPowerSlider = HUDui.Instance?.p2SuperSlider;
        }
    }

    public void IncreaseScore(int scoreAmount) {
        score += scoreAmount;
    }

    public void SetBasePowerSliderValue(float percent) {
        basePowerSlider.value = percent;
    }

    public void SetSpecialPowerSliderValue(float percent) {
        specialPowerSlider.value = percent;
    }
}
