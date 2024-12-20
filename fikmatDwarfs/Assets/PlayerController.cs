using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    PlayerObject PlayerOne;
    PlayerObject PlayerTwo;
    GamePlayLogic gamePlayLogic;

    void Awake() {
        gamePlayLogic = GamePlayLogic.Instance;
    }

    public void Initialize() {
        PlayerOne = gamePlayLogic.PlayerOne;
        PlayerTwo = gamePlayLogic.PlayerTwo;
    }

    void OnEnable() {
        InputManager.Instance?.SubscribeToAction("PrimaryOne", OnPrimaryOne);
        InputManager.Instance?.SubscribeToAction("SecondaryOne", OnSecondaryOne);
        InputManager.Instance?.SubscribeToAction("MoveOne", OnMoveOne, true);
        InputManager.Instance?.SubscribeToAction("MoveTwo", OnMoveTwo, true);
        InputManager.Instance?.SubscribeToAction("PrimaryTwo", OnPrimaryTwo);
        InputManager.Instance?.SubscribeToAction("SecondaryTwo", OnSecondaryTwo);
    }

    void OnDisable() {
        InputManager.Instance?.UnsubscribeFromAction("PrimaryOne", OnPrimaryOne);
        InputManager.Instance?.UnsubscribeFromAction("SecondaryOne", OnSecondaryOne);
        InputManager.Instance?.UnsubscribeFromAction("MoveOne", OnMoveOne, true);
        InputManager.Instance?.UnsubscribeFromAction("MoveTwo", OnMoveTwo, true);
        InputManager.Instance?.SubscribeToAction("PrimaryTwo", OnPrimaryTwo);
        InputManager.Instance?.SubscribeToAction("SecondaryTwo", OnSecondaryTwo);
    }

    void OnPrimaryOne(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;
        if (context.performed) {
            ScreenEffectManager.Instance.ScreenShakeImpulse(NoiseSetting.ProfileSixdShake, 5f, 5f, 0.2f);
        }
    }

    void OnSecondaryOne(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;
        if (context.performed) {
            print("secondary pressed");
            print($"gameover is {gamePlayLogic.gameOver}");
            if (gamePlayLogic.gameOver) gamePlayLogic.ResetGame();
        }
    }

    void OnPrimaryTwo(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;
        if (context.performed) {
            ScreenEffectManager.Instance.ScreenShakeImpulse(NoiseSetting.ProfileSixdShake, 10f, 10f, 0.2f);
        }
    }

    void OnSecondaryTwo(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;
        if (context.performed) {
            if (gamePlayLogic.gameOver) gamePlayLogic.ResetGame();
        }
    }

    void OnMoveOne(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;
        Vector2 moveDirectionOne;
        if (context.performed) {
            moveDirectionOne = context.ReadValue<Vector2>();
        } else if (context.canceled) {
            moveDirectionOne = Vector2.zero;
        }
        PlayerOne.Move(context.ReadValue<Vector2>());
    }

    void OnMoveTwo(InputAction.CallbackContext context) {
        if (!gamePlayLogic.gameStarted) return;

        Vector2 moveDirectionTwo;
        if (context.performed) {
            moveDirectionTwo = context.ReadValue<Vector2>();
        } else if (context.canceled) {
            moveDirectionTwo = Vector2.zero;
        }
        PlayerTwo.Move(context.ReadValue<Vector2>());
    }
}
