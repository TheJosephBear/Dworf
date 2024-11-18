using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    int playerOneScore = 0;
    int playerTwoScore = 0;

    void Awake() {
        UpdateScore();
        LoadHighScore();  // Load the high score when the game starts
    }

    void UpdateScore() {
        HUDui.Instance?.SetScoreOne(playerOneScore);
        HUDui.Instance?.SetScoreTwo(playerTwoScore);
        UpdateHighScore();  // Check if new high score needs updating
    }

    public void IncreaseScore(bool player, int score) {
        if (player) {
            playerOneScore += score;
        } else {
            playerTwoScore += score;
        }
        UpdateScore();
    }

    void UpdateHighScore() {
        int higherCurrentScore = GetHigherScore();
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (higherCurrentScore > highScore) {
            PlayerPrefs.SetInt("HighScore", higherCurrentScore);
            PlayerPrefs.Save();
        }
    }

    public int GetHighScore() {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

    public int GetHigherScore() {
        return Mathf.Max(playerOneScore, playerTwoScore);
    }

    void LoadHighScore() {
        // Optionally, load the high score and display it on the HUD if needed
        int highScore = GetHighScore();
        Debug.Log("Loaded High Score: " + highScore);
    }
}
