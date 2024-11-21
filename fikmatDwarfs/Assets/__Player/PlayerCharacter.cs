using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    public int hp = 2;
    public float speedMultiplier = 1f;
    public float allScoreGainMulitplier = 1f;
    public float passiveScoreGainMulitplier = 1f;
    public float passiveScoreGainSpeed = 1f;
    public float holeSpawnSpeed = 0.1f;
    public GameObject characterSelectSprite;
    public GameObject diggingEffect;
    public GameObject holeImage;

    // Power effects
    [Header("Power effects")]
    public GameObject shieldEffectGameObject;
    public GameObject fastEffectGameObject;
    public GameObject slowEffectGameObject;
    public GameObject loseControlEffectGameObject;
    public GameObject scoreStolenEffectGameObject;
    public GameObject teleportedEffectGameObject;
    public GameObject moneyMakerEffectGameObject;

    [Header("Privates")]
    public Player playerOwner;
    Rigidbody2D rb;
    public bool canControl = true;
    public bool isDead = false;
    public Vector2 direction;
    float scoreTimer;
    float nextSpawnTime = 0f;
    StuffSpawner spawner;
    bool invincible = false;

    float originalSpeedMultip;

    List<Coroutine> speedAlteringCoroutines = new List<Coroutine>();
    List<GameObject> speedAlteringEffects = new List<GameObject>();

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spawner = FindAnyObjectByType<StuffSpawner>();
        ToggleDiggingEffect(false);

        originalSpeedMultip = speedMultiplier;
    }

    void Update() {
        if (isDead) return;
        MoveFr();
        PassiveScoreGain();
        SpawnHoleEffect();
        UpdatePowerSliders();
    }

    #region INPUT public functions

    public void CastBasePower() {
        GetComponent<IbasePower>()?.Cast();
    }

    public void CastSpecialPower() {
        GetComponent<IsuperPower>()?.Cast();
    }

    public void Move(Vector2 moveInput) {
        direction = moveInput;
    }

    #endregion


    #region Power related functions

    public void GetScoreGainUp(float duration, float multiplier) {
        StartCoroutine(ScoreGainUpCoroutine(duration, multiplier));
    }

    IEnumerator ScoreGainUpCoroutine(float time, float multiplier) {
        GameObject eff = Instantiate(moneyMakerEffectGameObject, transform);
        allScoreGainMulitplier += multiplier;
        float ogPassive = passiveScoreGainSpeed;
        passiveScoreGainSpeed *= 0.1f;
        yield return new WaitForSeconds(time);
        allScoreGainMulitplier -= multiplier;
        passiveScoreGainSpeed = ogPassive;
        Destroy(eff);
    }

    // Lose control
    public void LoseControl(float time) {
        StartCoroutine(LoseControlCoroutine(time));
    }

    IEnumerator LoseControlCoroutine(float time) {
        GameObject eff = Instantiate(loseControlEffectGameObject, transform);
        canControl = false;
        yield return new WaitForSeconds(time);
        canControl = true;
        Destroy(eff);
    }

    // Get teleported
    public void GetTeleported(Vector3 newPosition) {
        Instantiate(teleportedEffectGameObject, transform.position, Quaternion.identity);
        transform.position = newPosition;
    }

    // Get Slowed
    public void GetSlowed(float amount, float duration) {
        speedAlteringCoroutines.Add(StartCoroutine(GetSlowedCoroutine(amount, duration)));
    }

    IEnumerator GetSlowedCoroutine(float amount, float duration) {
        GameObject eff = Instantiate(slowEffectGameObject, transform);
        speedAlteringEffects.Add(eff);
        speedMultiplier -= amount;
        yield return new WaitForSeconds(duration);
        speedMultiplier += amount;
        Destroy(eff);
    }

    // Speed up 
    public void GetFast(float amount, float duration) {
        speedAlteringCoroutines.Add(StartCoroutine(GetFastCoroutine(amount, duration)));
    }

    IEnumerator GetFastCoroutine(float amount, float duration) {
        GameObject eff = Instantiate(fastEffectGameObject, transform);
        speedAlteringEffects.Add(eff);
        speedMultiplier += amount;
        yield return new WaitForSeconds(duration);
        speedMultiplier -= amount;
        Destroy(eff);
    }

    public void RestoreBaseSpeed() {
        foreach (Coroutine coroutine in speedAlteringCoroutines) {
            StopCoroutine(coroutine);
        }
        foreach (GameObject go in speedAlteringEffects) {
            Destroy(go);
        }
        speedMultiplier = originalSpeedMultip;
    }

    // Score steal
    public void GetScoreStolen(int amount) {
        Instantiate(scoreStolenEffectGameObject, transform);
        playerOwner.score -= amount;
        if (playerOwner.score < 0) { playerOwner.score = 0; }   
    }

    // Gain shield
    public void GetShield(float duration) {
        StartCoroutine(GetShieldCoroutine(duration));
    }

    IEnumerator GetShieldCoroutine(float duration) {
        GameObject eff = Instantiate(shieldEffectGameObject, transform);
        invincible = true;
        yield return new WaitForSeconds(duration);
        invincible = false;
        Destroy(eff);
    }

    #endregion


    public void GameStarted() {
        ToggleDiggingEffect(true);
    }

    void UpdatePowerSliders() {
        playerOwner.SetBasePowerSliderValue(GetComponent<IbasePower>().GetMeterPercent());
        playerOwner.SetSpecialPowerSliderValue(GetComponent<IsuperPower>().GetMeterPercent());
    }

    // movement logic 
    void MoveFr() {
        if (!canControl) return;

        // Calculate the desired velocity
        Vector2 desiredVelocity = direction * GamePlayLogic.Instance.basePlayerMovementSpeed * speedMultiplier;

        // Interpolate between the current velocity and the desired velocity for smooth transitions
        Vector2 smoothedVelocity = Vector2.Lerp(rb.velocity, desiredVelocity, Time.deltaTime * 10f);

        // Apply the smoothed velocity using Rigidbody2D's physics system
        rb.velocity = smoothedVelocity;

        // Stop movement completely if no input and external forces are negligible
        if (direction == Vector2.zero && rb.velocity.magnitude < 0.1f) {
            rb.velocity = Vector2.zero;
        }
    }


    void ToggleDiggingEffect(bool toggleOn) {
        diggingEffect.SetActive(toggleOn);
    }

    void SpawnHoleEffect() {
        if (!GamePlayLogic.Instance.gameStarted) return;
    //    if (direction.y > 0) return;
        if (Time.time >= nextSpawnTime) {
            spawner.SpawnObjectToMoveWithWall(holeImage, transform.position);
            nextSpawnTime = Time.time + holeSpawnSpeed;
        }

    }





    void PassiveScoreGain() {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= passiveScoreGainSpeed) {
            GainScore((int)(1* passiveScoreGainMulitplier));
            scoreTimer = 0f;
        }
    }

    public void GainScore(int score) {
        FindAnyObjectByType<Stats>().IncreaseScore(playerOwner, (int)(score * allScoreGainMulitplier));
    }

    public void GetHit(int damage) {
        if (invincible) return;
        hp -= damage;
        if(hp <= 0 ) {
            Die();
        }
    }
    
    public void Die() {
        isDead = true;
        gameObject.SetActive(false);
    }


    public Player GetPlayer() {
        return playerOwner;
    }


    void OnCollisionEnter2D(Collision2D collision) {
        // Dampen velocity on collision rather than resetting it to zero
        rb.velocity *= 0.5f;

        // Optional: Reset external forces if needed
        direction = Vector2.zero;
    }
}
