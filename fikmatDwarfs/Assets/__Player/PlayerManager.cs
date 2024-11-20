using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager> {

    public Player PlayerOne;
    public Player PlayerTwo;
    public List<PlayerCharacter> characterList = new List<PlayerCharacter>();


    #region Input

    // Player ONE
    public void OnPrimaryOne() {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:
                FindAnyObjectByType<StartScreenLogic>().SkipIntro();
                break;
            case GameState.PlayerSelect:
                PlayerSelectLogic.Instance.ConfirmSelection(true);
                break;
            case GameState.GamePlay:
                PlayerOne.PlayerCharacter.CastBasePower();
                break;
            case GameState.GameOver:

                break;
        }
    }

    public void OnSecondaryOne() {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:
                FindAnyObjectByType<StartScreenLogic>().SkipIntro();

                break;
            case GameState.PlayerSelect:
                PlayerSelectLogic.Instance.ConfirmSelection(true);

                break;
            case GameState.GamePlay:
                PlayerOne.PlayerCharacter.CastSpecialPower();

                break;
            case GameState.GameOver:

                break;
        }

    }

    public void OnMoveOne(Vector2 moveInput) {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:

                break;
            case GameState.PlayerSelect:
                if (moveInput.x == 0) return;
                bool next = false;
                if (moveInput.x > 0) next = true;
                if (moveInput.x < 0) next = false;
                PlayerSelectLogic.Instance.ChangePlayerCharacter(true, next);
                break;
            case GameState.GamePlay:
                PlayerOne.PlayerCharacter.Move(moveInput);

                break;
            case GameState.GameOver:

                break;
        }

    }

    // Player TWO
    public void OnPrimaryTwo() {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:
                FindAnyObjectByType<StartScreenLogic>().SkipIntro();

                break;
            case GameState.PlayerSelect:
                PlayerSelectLogic.Instance.ConfirmSelection(false);

                break;
            case GameState.GamePlay:
                PlayerTwo.PlayerCharacter.CastBasePower();

                break;
            case GameState.GameOver:

                break;
        }

    }

    public void OnSecondaryTwo() {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:
                FindAnyObjectByType<StartScreenLogic>().SkipIntro();

                break;
            case GameState.PlayerSelect:
                PlayerSelectLogic.Instance.ConfirmSelection(false);

                break;
            case GameState.GamePlay:
                PlayerTwo.PlayerCharacter.CastSpecialPower();

                break;
            case GameState.GameOver:

                break;
        }

    }

    public void OnMoveTwo(Vector2 moveInput) {
        switch (GameManager.Instance.currentState) {
            case GameState.IntroCutscene:

                break;
            case GameState.PlayerSelect:
                if (moveInput.x == 0) return;
                bool next = false;
                if (moveInput.x > 0) next = true;
                if (moveInput.x < 0) next = false;
                PlayerSelectLogic.Instance.ChangePlayerCharacter(true, next);
                break;
            case GameState.GamePlay:
                PlayerTwo.PlayerCharacter.Move(moveInput);

                break;
            case GameState.GameOver:

                break;
        }

    }

    #endregion

}
