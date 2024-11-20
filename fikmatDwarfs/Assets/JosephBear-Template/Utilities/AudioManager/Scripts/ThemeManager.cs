using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeManager : MonoBehaviour {
    public static ThemeManager Instance { get; private set; }

    private AudioManager audioManager;
    private SoundBoard soundBoard;
    private List<SoundType> activeThemes;
    private List<SoundType> pausedThemes;

    void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this.gameObject);
        } else {
            Instance = this;
            audioManager = AudioManager.Instance;
            soundBoard = SoundBoard.Instance;
            activeThemes = new List<SoundType>();
            pausedThemes = new List<SoundType>();
        }
    }

    public void PlayTheme(SoundType theme, bool fadeIn = false, float fadeDuration = 1.0f) {
        if (!activeThemes.Contains(theme)) {
            activeThemes.Add(theme);
            if (fadeIn) {
                AudioManager.Instance.PlaySound(theme, fadeDuration);
            }else {
                AudioManager.Instance.PlaySound(theme);
            }
        }
    }

    public void StopTheme(SoundType theme, bool fadeOut = false, float fadeDuration = 1.0f) {
        if (activeThemes.Contains(theme)) {
            if (fadeOut) {
                AudioManager.Instance.FadeOutSound(theme, fadeDuration);
            } else {
                AudioManager.Instance.StopSound(theme);
            }
            activeThemes.Remove(theme);
        }
    }

    public void StopAllThemes(bool fadeOut = false, float fadeDuration = 1.0f) {
        print("dropping all themes");
        foreach (SoundType theme in new List<SoundType>(activeThemes)) {
            StopTheme(theme, fadeOut, fadeDuration);
        }
        print("Themes left: "+activeThemes.Count);
    }

    public void PauseAllThemes() {
        pausedThemes.Clear();
        foreach (SoundType theme in activeThemes) {
            AudioManager.Instance.PauseSound(theme);
            pausedThemes.Add(theme);
        }
    }

    public void ResumeAllThemes() {
        foreach (SoundType theme in pausedThemes) {
            AudioManager.Instance.ResumeSound(theme);
        }
        pausedThemes.Clear();
    }
}
