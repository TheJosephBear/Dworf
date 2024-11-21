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
    public GameObject characterSelectSprite;
    public GameObject diggingEffect; // Permanently at the bottom of the character while digging
    public GameObject holeImage; // hole left behind while digging
    public float holeSpawnSpeed = 0.1f;
    public Player playerOwner;

    Rigidbody2D rb;
    public bool canControl = true;
    public bool isDead = false;
    Vector2 direction;
    float scoreTimer;
    float nextSpawnTime = 0f;
    StuffSpawner spawner;

    float originalSpeedMultip;


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

    // Lose control
    public void LoseControl(float time) {
        StartCoroutine(LoseControlCoroutine(time));
    }

    IEnumerator LoseControlCoroutine(float time) {
        canControl = false;
        yield return new WaitForSeconds(time);
        canControl = true;
    }

    // Get teleported
    public void GetTeleported(Vector3 newPosition) {
        transform.position = newPosition;
    }

    // Get Slowed
    public void GetSlowed(float amount, float duration) {
        StartCoroutine(GetSlowedCoroutine(amount, duration));
    }

    IEnumerator GetSlowedCoroutine(float amount, float duration) {
        speedMultiplier = amount;
        yield return new WaitForSeconds(duration);
        speedMultiplier = originalSpeedMultip;
    }

    // Speed up 
    public void GetFast(float amount, float duration) {
        StartCoroutine(GetFastCoroutine(amount, duration));
    }

    IEnumerator GetFastCoroutine(float amount, float duration) {
        speedMultiplier = amount;
        yield return new WaitForSeconds(duration);
        speedMultiplier = originalSpeedMultip;
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
        rb.velocity = direction * GamePlayLogic.Instance.basePlayerMovementSpeed * speedMultiplier;
        if (direction == Vector2.zero) {
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
        hp -= damage;
        if(hp <= 0 ) {
            Die();
        }
    }
    
    public void Die() {
        isDead = true;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Stop movement on collision by setting velocity to zero
        rb.velocity = Vector2.zero;
    }

    public Player GetPlayer() {
        return playerOwner;
    }
}
