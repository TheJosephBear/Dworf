using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour {

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
    public bool isDead = false;
    Vector2 direction;
    float scoreTimer;
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

    public void GameStarted() {
        ToggleDiggingEffect(true);
    }

    void UpdatePowerSliders() {
        playerOwner.SetBasePowerSliderValue(GetComponent<IbasePower>().GetMeterPercent());
        playerOwner.SetSpecialPowerSliderValue(GetComponent<IsuperPower>().GetMeterPercent());
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
        scoreTimer += Time.deltaTime;
        if (scoreTimer >= passiveScoreGainSpeed) {
            GainScore((int)(1* passiveScoreGainMulitplier));
            scoreTimer = 0f;
        }
    }

    public void GainScore(int score) {
        FindAnyObjectByType<Stats>().IncreaseScore(playerOwner, (int)(score * allScoreGainMulitplier));
    }

    public void Die() {
        isDead = true;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Stop movement on collision by setting velocity to zero
        rb.velocity = Vector2.zero;
    }

    public Player GetPlayer() {
        return playerOwner;
    }
}
