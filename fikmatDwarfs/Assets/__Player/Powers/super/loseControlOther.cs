using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class loseControlOther : MonoBehaviour, IsuperPower {

    public float powerDuration = 2f;
    public float meterMax = 1f;
    public float meterRegen = 0.1f;

    PlayerCharacter characterScript;
    float currentMeter;
    bool isMeterFull => currentMeter >= meterMax;
    bool isPowerActive = false;

    void Awake() {
        characterScript = GetComponent<PlayerCharacter>();
        currentMeter = meterMax; // Start with a full meter
    }

    void Update() {
        if (!isPowerActive) {
            RegenerateMeter();
        }
    }

    public void Cast() {
        if (isMeterFull) {
            StartCoroutine(CastCoroutine());
            currentMeter = 0f;
        } else {
            OnMeterNotReady();
        }
    }

    IEnumerator CastCoroutine() {
        isPowerActive = true;

        if (characterScript.playerOwner.isPlayerOne) {
            GamePlayLogic.Instance.p2Character.LoseControl(powerDuration);
        } else {
            GamePlayLogic.Instance.p1Character.LoseControl(powerDuration);
        }

        yield return new WaitForSeconds(powerDuration);

        isPowerActive = false;
    }

    void RegenerateMeter() {
        if (!isMeterFull) {
            currentMeter = Mathf.Min(currentMeter + meterRegen * Time.deltaTime, meterMax);
        }
    }

    void OnMeterNotReady() {

    }

    public float GetMeterPercent() {
        return currentMeter / meterMax;
    }
}