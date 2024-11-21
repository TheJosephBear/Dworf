using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SocialPlatforms.Impl;

public class GamePlayLogic : Singleton<GamePlayLogic> {

    public float introCutsceneSpeed = 2f;
  //  public PlayerObject PlayerOne;
 //   public PlayerObject PlayerTwo;
    public Transform PlayerOneSpawn;
    public Transform PlayerTwoSpawn;
    public GameObject StuffSpawner;

    public GameObject PlayerOneObject;
    public GameObject PlayerTwoObject;

    public float basePlayerMovementSpeed = 10f;

    BackgroundMoving backgroundMoving;
    float elapsedTime = 0f;
    public bool gameStarted = false;
    public bool gameOver = false;

    PlayerCharacter p1Character;
    PlayerCharacter p2Character;


    protected override void Awake() {
        base.Awake();

        backgroundMoving = FindAnyObjectByType<BackgroundMoving>();
    }

    void Update() {
        GameOverCheck();
    }


    public void StartGame() {
        // Has to instantialize the players
        CreatePlayerCharacters();
        // play animation of them coming out -> enable the gameplay to start
        StartCoroutine(PlayIntroCutscene());

        /*
        print("GameplayLOGIC - StartGame");
        StartCoroutine(PlayIntroCutscene());
        */
    }

    void CreatePlayerCharacters() {
        if (FindAnyObjectByType<StartScreenLogic>().playerOneJoined) {
            PlayerOneObject = SceneLoadingManager.Instance.InstantiateObjectInScene(PlayerManager.Instance.PlayerOne.PlayerCharacter.gameObject, PlayerOneSpawn.position, SceneType.MainMenu);
            p1Character = PlayerOneObject.GetComponent<PlayerCharacter>();
            p1Character.playerOwner = PlayerManager.Instance.PlayerOne;
        }
        if (FindAnyObjectByType<StartScreenLogic>().playerTwoJoined) {
            PlayerTwoObject = SceneLoadingManager.Instance.InstantiateObjectInScene(PlayerManager.Instance.PlayerTwo.PlayerCharacter.gameObject, PlayerTwoSpawn.position, SceneType.MainMenu);
            p2Character = PlayerTwoObject.GetComponent<PlayerCharacter>();
            p2Character.playerOwner = PlayerManager.Instance.PlayerTwo;
        }
    }

    IEnumerator PlayIntroCutscene() {
        print("intro gampley cutscene started");
        ThemeManager.Instance.StopAllThemes(true, 2f);
        ThemeManager.Instance.PlayTheme(SoundType.main_theme, true, 1f);
        if (PlayerOneObject != null) PlayerOneObject?.transform.DOMoveY(PlayerOneSpawn.position.y - 5f, introCutsceneSpeed);
        if (PlayerTwoObject!=null) PlayerTwoObject.transform.DOMoveY(PlayerTwoSpawn.position.y - 5f, introCutsceneSpeed);
        yield return new WaitForSeconds(introCutsceneSpeed);
        ActualGameStart();
    }

    void ActualGameStart() {
        print("GameStarted");
        backgroundMoving.ToggleMovement(true);
        UImanager.Instance.ShowUI(UIType.HUD);
        HUDui.Instance?.SetScoreOne(0);
        HUDui.Instance?.SetScoreTwo(0);
        GameManager.Instance.ChangeGameState(GameState.GamePlay);
        gameStarted = true;
        p1Character?.GameStarted();
        p2Character?.GameStarted();
    }




    void GameOverCheck() {
        if (!gameStarted) return;
        if (gameOver) return;
        if ((p2Character != null && p1Character == null && p2Character.isDead) || (p1Character != null && p2Character == null && p1Character.isDead) || (p1Character.isDead && p2Character.isDead)) {
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine() {
        print("gameover coroutine, gameover should be be true if this is called second time" + gameOver);
        gameStarted = false;
        yield return new WaitForSeconds(.7f);
        gameOver = true;
        GameManager.Instance.ChangeGameState(GameState.GameOver);   
        // Show try again UI
        UImanager.Instance.ShowUI(UIType.GameOverScreen);
        GameOverUI.Instance.ShowEndScore(FindAnyObjectByType<Stats>().GetHighScore(), FindAnyObjectByType<Stats>().GetHigherScore());
    }

    public void ResetGame() {
        print("Reset game called");
        GameManager.Instance.ChangeGameState(GameState.ButtonIsPushed);
        UImanager.Instance.HideUI(UIType.GameOverScreen);
        gameOver = false;
        gameStarted = false;
        UImanager.Instance.HideUI(UIType.HUD);
        GameResettingManager.Instance.ResetGame();
    }
}