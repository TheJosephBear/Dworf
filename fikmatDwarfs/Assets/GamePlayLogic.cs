using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class GamePlayLogic : Singleton<GamePlayLogic> {

    public PlayableDirector introCutscene;
    public PlayerObject PlayerOnePrefab;
    public PlayerObject PlayerTwoPrefab;
    public PlayerObject PlayerOne;
    public PlayerObject PlayerTwo;
    public Transform PlayerOneSpawn;
    public Transform PlayerTwoSpawn;
    public GameObject StuffSpawner;

    public float basePlayerMovementSpeed = 10f;

    BackgroundMoving backgroundMoving;
    float elapsedTime = 0f;
    public bool gameStarted = false;
    public bool gameOver = false;


    protected override void Awake() {
        base.Awake();

        backgroundMoving = FindAnyObjectByType<BackgroundMoving>();
    }

    void Update() {
        GameOverCheck();
    }

    public void StartGame() {
        print("GameplayLOGIC - StartGame");
        StartCoroutine(PlayIntroCutscene());
    }

    void GameOverCheck() {
        if (!gameStarted) return;

        if (PlayerOne.isDead && PlayerTwo.isDead) {
            print("yall is dead");
            StartCoroutine(GameOverCoroutine());
        }
    }

    IEnumerator GameOverCoroutine() {
        yield return new WaitForSeconds(.7f);
        gameOver = true;
        // Show try again UI
        HUDui.Instance.ToggleGameOver(true);
        HUDui.Instance.ShowEndScore(FindAnyObjectByType<Stats>().GetHighScore(), FindAnyObjectByType<Stats>().GetHigherScore());
    }

    public void ResetGame() {
        HUDui.Instance.ToggleGameOver(false);
        gameOver = false;
        gameStarted = false;
        UImanager.Instance.HideUI(UIType.HUD);
        GameResettingManager.Instance.ResetGame();
    }

    IEnumerator PlayIntroCutscene() {
        // Play cutscene of dwarfs going into the ground
        print("play intro cutscene");
        introCutscene.Play();
        print("stop the themes");
        ThemeManager.Instance.StopAllThemes(true, 2f);
        print("play the main theme");
        ThemeManager.Instance.PlayTheme(SoundType.main_theme, true, 1f);
        while (introCutscene.state == PlayState.Playing) {
            yield return null;
        }
        introCutscene.gameObject.SetActive(false);
        // Instantiate actual player objects and start game
        EnableGameObjects();
        ActualGameStart();
    }

    void ActualGameStart() {
        print("move background");
        backgroundMoving.ToggleMovement(true);
        print("Initialize player controller");
        FindAnyObjectByType<PlayerController>().Initialize();
        print("show hud");
        UImanager.Instance.ShowUI(UIType.HUD);
        print("hud gameover screen hidden! ");
        HUDui.Instance.ToggleGameOver(false);
        HUDui.Instance?.SetScoreOne(0);
        HUDui.Instance?.SetScoreTwo(0);
        print(" game started");
        gameStarted = true;
    }

    void EnableGameObjects() {
        print("instantiating the players");
        // Players
        PlayerOne = SceneLoadingManager.Instance.InstantiateObjectInScene(PlayerOnePrefab.gameObject, PlayerOneSpawn.position, SceneType.MainMenu).GetComponent<PlayerObject>();
        PlayerTwo = SceneLoadingManager.Instance.InstantiateObjectInScene(PlayerTwoPrefab.gameObject, PlayerTwoSpawn.position, SceneType.MainMenu).GetComponent<PlayerObject>();
    }

   
}