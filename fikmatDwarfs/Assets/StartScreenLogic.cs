using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class StartScreenLogic : MonoBehaviour {

    public PlayableDirector introCutscene;
    public GameObject playerJoinUI; // Canvas object in world space
    public GameObject cameraIntro;
    public TextMeshProUGUI playerOneMessage;
    public TextMeshProUGUI playerTwoMessage;
    public TextMeshProUGUI countdownText;
    public Vector3 CameraAnimationTarget;

    public bool playerOneJoined = false;
    public bool playerTwoJoined = false;
    public float countdownTime = 5f; // Countdown in seconds
    private Coroutine countdownCoroutine;
    private Coroutine introAnimCoroutine;
    bool introPlaying = true;


    private bool gameStartingSafe = false;

    public void EnterTheScreen() {
        playerJoinUI.SetActive(false);
        GameManager.Instance.ChangeGameState(GameState.IntroCutscene);
        ThemeManager.Instance.StopAllThemes(true, 1f);
        ThemeManager.Instance.PlayTheme(SoundType.intro, true, 1f);
        // AnimationFirst
        introAnimCoroutine = StartCoroutine(PlayIntroCutscene());
    }

    IEnumerator PlayIntroCutscene() {
        // Play cutscene of dwarfs going into the ground
        introPlaying = true;
        introCutscene.Play();
        while (introCutscene.state == PlayState.Playing) {
            yield return null;
        }
        // introCutscene.gameObject.SetActive(false);
        // Show the UI for player choosing and keep animation frozen on last frame
        introPlaying = false;
        ShowStartUI();
        FindAnyObjectByType<PlayerSelectLogic>().Initialize();
    }

    public void SkipIntro() {
        if (!introPlaying) return;
        StopCoroutine(introAnimCoroutine);
        introCutscene.time = introCutscene.duration;
        introCutscene.Evaluate();
        introCutscene.Stop();
        introPlaying = false;
        ShowStartUI();
        FindAnyObjectByType<PlayerSelectLogic>().Initialize();
    }



    public void OnPlayerJoin(bool isPlayerOne) {
        ShowPlayerMessage(isPlayerOne, "Pøipraven");
        if (isPlayerOne) {
            playerOneJoined = true;
        } else {
            playerTwoJoined = true;
        }

        if (playerOneJoined && playerTwoJoined) {
            // Both players joined, stop countdown and start cutscene, after small pause
            if(!gameStartingSafe) StartCoroutine(StartAfterPause());
        } else if (playerOneJoined || playerTwoJoined) {
            // Start countdown when only player one has joined
            StartCountdown();
        }
    }

    IEnumerator StartAfterPause() {
        gameStartingSafe = true;
        countdownText.text = "";
        yield return new WaitForSeconds(.25f);
        StopCoroutine(countdownCoroutine);
        StartCoroutine(IntroCutscene());
    }

    void ShowStartUI() {
        UtilityUI.Fade(playerJoinUI, true, 0.2f);
        ShowPlayerMessage(true, "Stiskni èervené tlaèítko");
        ShowPlayerMessage(false, "Stiskni èervené tlaèítko");
    }
    void StartCountdown() {
        // Start countdown if not already started
        if (countdownCoroutine == null) {
            countdownCoroutine = StartCoroutine(CountdownCoroutine());
        }
    }

    void ShowPlayerMessage(bool isPlayerOne, string message) {
        if (isPlayerOne) {
            playerOneMessage.text = message;
        } else {
            playerTwoMessage.text = message;
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

    // moving camera down before game start
    IEnumerator IntroCutscene() {
        GameManager.Instance.ChangeGameState(GameState.ButtonIsPushed); //something so players cant input
        yield return new WaitForSeconds(1f);
        print("intro cutscene");

        // Animate UI u
        float animationTime = 1f;
        float elapsedTime = 0f;
        Vector3 startPosition = playerJoinUI.transform.position;

        while (elapsedTime < animationTime) {
            //    playerJoinUI.transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationTime);
            //    cameraIntro.transform.position = Vector3.Lerp(startPosition, CameraAnimationTarget, elapsedTime / animationTime);  // makes the UI goBALLISTIC for one frame???
            cameraIntro.transform.DOMoveY(CameraAnimationTarget.y, animationTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Shutting off the start menu UI
        playerJoinUI.SetActive(false);

        PlayerSelectLogic.Instance.DeletePlayerPreviews();
        // Start game
        GamePlayLogic.Instance.StartGame();
    }

    
}
