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
    public Player playerOwner;

    Rigidbody2D rb;
    bool isDead = false;
    Vector2 direction;
    

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (isDead) return;
        MoveFr();
        PassiveScoreGain();
    }

    public void CastBasePower() {

    }

    public void CastSpecialPower() {

    }

    public void Move(Vector2 moveInput) {
        direction = moveInput;
    }


    /* movement logic */
    void MoveFr() {
        rb.velocity = direction * GamePlayLogic.Instance.basePlayerMovementSpeed * speedMultiplier;
        if (direction == Vector2.zero) {
            rb.velocity = Vector2.zero;
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
