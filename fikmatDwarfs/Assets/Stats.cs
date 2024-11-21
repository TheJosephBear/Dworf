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
        if (PlayerManager.Instance != null && PlayerManager.Instance.PlayerOne != null && PlayerManager.Instance.PlayerTwo != null) {
            playerOneScore = PlayerManager.Instance.PlayerOne.score;
            playerTwoScore = PlayerManager.Instance.PlayerTwo.score;
            HUDui.Instance?.SetScoreOne(playerOneScore);
            HUDui.Instance?.SetScoreTwo(playerTwoScore);
            UpdateHighScore();  // Check if new high score needs updating
        }
    }

    public void IncreaseScore(Player player, int score) {
        player.IncreaseScore(score);
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
