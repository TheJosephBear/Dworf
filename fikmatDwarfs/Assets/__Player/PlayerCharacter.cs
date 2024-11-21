using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

    public float speedMultiplier = 1f;
    public float basePowerMeterMax = 1f;
    public float basePowerMeterRegen = 1f;
    public float specialPowerMeterMax = 1f;
    public float specialPowerMeterRegen = 1f;
    public GameObject characterSelectSprite;
    public GameObject diggingEffect; // Permanently at the bottom of the character while digging
    public GameObject holeImage; // hole left behind while digging
    public float holeSpawnSpeed = 0.1f;
    public Player playerOwner;

    Rigidbody2D rb;
    public bool isDead = false;
    Vector2 direction; 
    float nextSpawnTime = 0f;
    StuffSpawner spawner;


    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        spawner = FindAnyObjectByType<StuffSpawner>();
        ToggleDiggingEffect(false);
    }

    void Update() {
        if (isDead) return;
        MoveFr();
        PassiveScoreGain();
        SpawnHoleEffect();
    }

    #region INPUT public functions

    public void CastBasePower() {

    }

    public void CastSpecialPower() {

    }

    public void Move(Vector2 moveInput) {
        direction = moveInput;
    }

    #endregion

    public void GameStarted() {
        ToggleDiggingEffect(true);
    }


    // movement logic 
    void MoveFr() {
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
        /*
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= stats.passive_score_gain_speed) {
            FindAnyObjectByType<Stats>().IncreaseScore(PlayerOne, 1);
            scoreTimer = 0f;
        }
        */
    }

    public void GainScore(int score) {
        //    FindAnyObjectByType<Stats>().IncreaseScore(PlayerOne, score);
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
