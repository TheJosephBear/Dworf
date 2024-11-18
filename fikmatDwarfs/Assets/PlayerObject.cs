using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour {


    public bool PlayerOne;
    Vector2 direction;
    Rigidbody2D rb;
    float scoreTimer;
    public bool isDead = false;
    PlayerStats stats;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<PlayerStats>();
    }

    void Update() {
        if (isDead) return;
        MoveFr();
        PassiveScoreGain();
    }

    void PassiveScoreGain() {
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= stats.passive_score_gain_speed) {
            FindAnyObjectByType<Stats>().IncreaseScore(PlayerOne, 1);
            scoreTimer = 0f;
        }
    }

    public void GainScore(int score) {
        FindAnyObjectByType<Stats>().IncreaseScore(PlayerOne, score);
    }

    public void Die() {
        isDead = true;
        gameObject.SetActive(false);

    }

    public void Move(Vector2 moveVector) {
        direction = moveVector;
    }

    void MoveFr() {
        // Set the velocity of the Rigidbody based on the direction input
        rb.velocity = direction * GamePlayLogic.Instance.basePlayerMovementSpeed * stats.speed_mutliplier;

        // Stop the player movement if no input is provided
        if (direction == Vector2.zero) {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Stop movement on collision by setting velocity to zero
        rb.velocity = Vector2.zero;
    }
}
