using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedPower : MonoBehaviour, IbasePower {

    public float speedMultiplier = 3f;
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
        float originalMultiplier = characterScript.speedMultiplier;
        characterScript.speedMultiplier = speedMultiplier;

        yield return new WaitForSeconds(powerDuration);

        characterScript.speedMultiplier = originalMultiplier;
        isPowerActive = false; 
    }

    private void RegenerateMeter() {
        if (!isMeterFull) {
            currentMeter = Mathf.Min(currentMeter + meterRegen * Time.deltaTime, meterMax);
        }
    }

    private void OnMeterNotReady() {
        Debug.Log("Meter is not ready! Please wait for it to recharge.");
    }

    public float GetMeterPercent() {
        return currentMeter / meterMax;
    }
}
