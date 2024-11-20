using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerSelectLogic : Singleton<PlayerSelectLogic> {
    public GameObject selectingUI;
    public Transform PlayerOneSpriteTransform;
    public Transform PlayerTwoSpriteTransform;
    GameObject p1SelectSpriteInstantiated;
    GameObject p2SelectSpriteInstantiated;
    int p1selectedCharacterIndex = 0;
    int p2selectedCharacterIndex = 0;

    protected override void Awake() {
        base.Awake();

        selectingUI.SetActive(false);
    }

    public void Initialize() {
        GameManager.Instance.ChangeGameState(GameState.PlayerSelect);
        selectingUI.SetActive(true); 
        // default characters
        PlayerManager.Instance.PlayerOne.PlayerCharacter = PlayerManager.Instance.characterList[p1selectedCharacterIndex];
        ChangePlayerSprite(true);
        PlayerManager.Instance.PlayerTwo.PlayerCharacter = PlayerManager.Instance.characterList[p2selectedCharacterIndex];
        ChangePlayerSprite(false);
    }

    void StartGame() {
        // Trigger game start
        FindAnyObjectByType<GamePlayLogic>().StartGame();
    }

    public void ChangePlayerCharacter(bool playerOne, bool next) {
        // Add time out for each player because it can be called continuosly
        if (playerOne) {
            if (next) {
                p1selectedCharacterIndex++;
            } else {
                p1selectedCharacterIndex--;
            }
            PlayerManager.Instance.PlayerOne.PlayerCharacter = PlayerManager.Instance.characterList[p1selectedCharacterIndex];
            ChangePlayerSprite(playerOne);
        } else {
            if (next) {
                p2selectedCharacterIndex++;
            } else {
                p2selectedCharacterIndex--;
            }
            PlayerManager.Instance.PlayerTwo.PlayerCharacter = PlayerManager.Instance.characterList[p2selectedCharacterIndex];
            ChangePlayerSprite(playerOne);
        }
    }

    public void ConfirmSelection(bool playerOne) {

    }

    void ChangePlayerSprite(bool playerOne) {
        if (playerOne) {
            p1SelectSpriteInstantiated = Instantiate(PlayerManager.Instance.PlayerOne.PlayerCharacter.characterSelectSprite, PlayerOneSpriteTransform.position, Quaternion.identity);
        } else {
            p2SelectSpriteInstantiated = Instantiate(PlayerManager.Instance.PlayerTwo.PlayerCharacter.characterSelectSprite, PlayerTwoSpriteTransform.position, Quaternion.identity);
        }
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
