using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSlower : MonoBehaviour, IbasePower {

    public float powerDuration = 0.1f;
    public float meterMax = 1f;
    public float meterRegen = 0.1f;
    public GameObject SlowerGameObject;

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

        FindAnyObjectByType<StuffSpawner>().SpawnObjectToMoveWithWall(SlowerGameObject, transform.position + new Vector3(0, 2f, 0));

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