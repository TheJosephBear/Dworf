using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swapPlaces : MonoBehaviour, IbasePower {

    public float powerDuration = 0.1f;
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
            Vector3 tempPosition = GamePlayLogic.Instance.p1Character.gameObject.transform.position;
            GamePlayLogic.Instance.p1Character.GetTeleported(GamePlayLogic.Instance.p2Character.gameObject.transform.position);
            GamePlayLogic.Instance.p2Character.GetTeleported(tempPosition);
        } else {
            Vector3 tempPosition = GamePlayLogic.Instance.p2Character.gameObject.transform.position;
            GamePlayLogic.Instance.p2Character.GetTeleported(GamePlayLogic.Instance.p1Character.gameObject.transform.position);
            GamePlayLogic.Instance.p1Character.GetTeleported(tempPosition);
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
