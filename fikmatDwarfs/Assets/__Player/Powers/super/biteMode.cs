using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class biteMode : MonoBehaviour, IsuperPower {

    public int scoreStolen = 50;
    public float slowInflictedAmount = 0.5f;
    public float slowDuration = 0.3f;
    public float fastAmount = 0.3f;
    public float powerDuration = 3f;
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

        characterScript.GetFast(fastAmount, powerDuration);

        yield return new WaitForSeconds(powerDuration);


        isPowerActive = false;
    }

    void Bite() {
        if (characterScript.playerOwner.isPlayerOne) {
            GamePlayLogic.Instance.p2Character.GetScoreStolen(scoreStolen);
            GamePlayLogic.Instance.p2Character.GetSlowed(slowInflictedAmount, slowDuration);
        } else {
            GamePlayLogic.Instance.p1Character.GetScoreStolen(scoreStolen);
            GamePlayLogic.Instance.p1Character.GetSlowed(slowInflictedAmount, slowDuration);
        }
        characterScript.playerOwner.IncreaseScore(scoreStolen);
        characterScript.RestoreBaseSpeed();
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

    void OnCollisionEnter2D(UnityEngine.Collision2D collision) {
        if (collision.transform.CompareTag("Player")) {
            if(isPowerActive) Bite();
        }
    }
}
