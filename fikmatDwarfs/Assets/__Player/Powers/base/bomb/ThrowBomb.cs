using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowBomb : MonoBehaviour, IbasePower {

    public GameObject bombPrefab;
    public float explodeAfterSeconds = 0.3f;
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

        // instantiate bomb, initialize bomb, it explodes by itself, has its own script fort ushing
        SceneLoadingManager.Instance.InstantiateObjectInScene(bombPrefab, transform.position, SceneType.MainMenu).GetComponent<ForceBomb>().LightTheFuse(explodeAfterSeconds, characterScript.direction);

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
