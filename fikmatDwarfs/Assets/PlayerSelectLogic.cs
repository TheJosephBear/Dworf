using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSelectLogic : Singleton<PlayerSelectLogic> {

    public GameObject selectingUI;

    protected override void Awake() {
        base.Awake();
        print("WEEEEEEEEEE WOOOOOOOOOOOO SETTING IT TO FALSE" + selectingUI.name);
        selectingUI.SetActive(false);
    }

    public void Initialize() {
        print("Good boy");
        selectingUI.SetActive(true);
    }

    void StartGame() {
        // Trigger game start
        FindAnyObjectByType<GamePlayLogic>().StartGame();
    }


    /*
    void ShowStartUI() {
        print("ShowStartUI");
        UtilityUI.Fade(playerJoinUI, true, 0.2f);
        ShowPlayerMessage(true, "Stiskni èervené tlaèítko");
        ShowPlayerMessage(false, "Stiskni èervené tlaèítko");
    }

    void ShowPlayerMessage(bool isPlayerOne, string message) {
        if (isPlayerOne) {
            playerOneMessage.text = message;
        } else {
            playerTwoMessage.text = message;
        }
    }

    void StartCountdown() {
        // Start countdown if not already started
        if (countdownCoroutine == null) {
            countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }
    }

    IEnumerator CountdownCoroutine() {
        float timeRemaining = countdownTime;
        while (timeRemaining >= 0) {
            yield return new WaitForSeconds(1f);
            countdownText.text = ((int)timeRemaining).ToString();
            timeRemaining--;
        }

        // Proceed to start cutscene once countdown ends or both players are ready
        StartCoroutine(IntroCutscene());
    }

    IEnumerator IntroCutscene() {
        print("intro cutscene");
        playerJoinUI.SetActive(false);
        // Example animation to move the canvas out of the screen

        // Animate UI u
        float animationTime = 1f;
        float elapsedTime = 0f;
        Vector3 startPosition = playerJoinUI.transform.position;

        while (elapsedTime < animationTime) {
            //    playerJoinUI.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationTime);
            cameraIntro.transform.position = Vector3.Lerp(startPosition, CameraAnimationTarget, elapsedTime / animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        print("Camera shoupld be looking down");

        // trigger player select HEHERHEHRHERHERHEHRHERHEHREHRHE
        FindAnyObjectByType<PlayerSelectLogic>().Initialize();

    }

    void OnPlayerJoin(bool isPlayerOne) {
        if (introPlaying) {
            SkipIntro();
            return;
        }

        ShowPlayerMessage(isPlayerOne, "Pøipraven");
        if (isPlayerOne) {
            playerOneJoined = true;
        } else {
            playerTwoJoined = true;
        }

        if (playerOneJoined && playerTwoJoined) {
            // Both players joined, stop countdown and start cutscene
            //   StopCoroutine(countdownCoroutine);
            //   StartCoroutine(IntroCutscene());
        } else if (playerOneJoined || playerTwoJoined) {
            // Start countdown when only player one has joined
            StartCountdown();
        }
    }

    void OnEnable() {
        InputManager.Instance?.SubscribeToAction("SecondaryOne", OnPrimaryOne);
        InputManager.Instance?.SubscribeToAction("SecondaryTwo", OnPrimaryTwo);
    }

    void OnDisable() {
        InputManager.Instance?.UnsubscribeFromAction("SecondaryOne", OnPrimaryOne);
        InputManager.Instance?.UnsubscribeFromAction("SecondaryTwo", OnPrimaryTwo);
    }

    void OnPrimaryOne(InputAction.CallbackContext context) {
        if (context.ReadValue<float>() == 1f && !playerOneJoined) {
            OnPlayerJoin(true);
        }
    }

    void OnPrimaryTwo(InputAction.CallbackContext context) {
        if (context.ReadValue<float>() == 1f && !playerTwoJoined) {
            OnPlayerJoin(false);
        }
    }

    */
}
