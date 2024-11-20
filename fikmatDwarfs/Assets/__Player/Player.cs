using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int score = 0;
    public bool isReady = false;
    public bool isAlive = true;
    public PlayerCharacter PlayerCharacter; // prefab

    public void IncreaseScore(int scoreAmount) {
        score += scoreAmount;   
    }
    
}
