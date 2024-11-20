using System.Collections;
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

    private bool gameStarted = false;
    private bool playerOneJoined = false;
    private bool playerTwoJoined = false;
    public float countdownTime = 5f; // Countdown in seconds
    private Coroutine countdownCoroutine;
    private Coroutine introAnimCoroutine;
    bool introPlaying = true;

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
        FindAnyObjectByType<PlayerSelectLogic>().Initialize();
    }

    public void SkipIntro() {
        if (!introPlaying) return;
        StopCoroutine(introAnimCoroutine);
        introCutscene.time = introCutscene.duration;
        introCutscene.Evaluate();
        introCutscene.Stop();
        introPlaying = false;
        print(FindAnyObjectByType<PlayerSelectLogic>());
        FindAnyObjectByType<PlayerSelectLogic>().Initialize();
    }

}
