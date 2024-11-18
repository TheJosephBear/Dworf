using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class ScreenEffectManager : Singleton<ScreenEffectManager> {

    public NoiseSettings ProfileSixdShake;
    public NoiseSettings ProfileSixdWobble;
    public NoiseSettings ProfileHandheldNormalMild;
    public NoiseSettings ProfileHandheldNormalStrong;
    public NoiseSettings ProfileHandheldNormalExtreme;
    public NoiseSettings ProfileHandheldTeleMild;
    public NoiseSettings ProfileHandheldTeleStrong;
    public NoiseSettings ProfileHandheldWideAngleMild;
    public NoiseSettings ProfileHandheldWideAngleStrong;

    private Camera m_camera;
    private Dictionary<NoiseSetting, NoiseSettings> noiseSettingsDictionary;
    private NoiseSetting defaultScreenShakeSetting = NoiseSetting.None; // Default screen shake setting

    protected override void Awake() {
        base.Awake();
        m_camera = FindAnyObjectByType<Camera>();
        InitializeNoiseSettingsDictionary();
    }

    private void InitializeNoiseSettingsDictionary() {
        noiseSettingsDictionary = new Dictionary<NoiseSetting, NoiseSettings> {
            { NoiseSetting.ProfileSixdShake, ProfileSixdShake },
            { NoiseSetting.ProfileSixdWobble, ProfileSixdWobble },
            { NoiseSetting.ProfileHandheldNormalMild, ProfileHandheldNormalMild },
            { NoiseSetting.ProfileHandheldNormalStrong, ProfileHandheldNormalStrong },
            { NoiseSetting.ProfileHandheldNormalExtreme, ProfileHandheldNormalExtreme },
            { NoiseSetting.ProfileHandheldTeleMild, ProfileHandheldTeleMild },
            { NoiseSetting.ProfileHandheldTeleStrong, ProfileHandheldTeleStrong },
            { NoiseSetting.ProfileHandheldWideAngleMild, ProfileHandheldWideAngleMild },
            { NoiseSetting.ProfileHandheldWideAngleStrong, ProfileHandheldWideAngleStrong },
            { NoiseSetting.None, null } // No shake
        };
    }

    private CinemachineBasicMultiChannelPerlin getNoise() {
        CinemachineBrain cmBrain = m_camera.GetComponent<CinemachineBrain>();
        ICinemachineCamera vcam = cmBrain.ActiveVirtualCamera;
        CinemachineVirtualCamera cineVcam = vcam as CinemachineVirtualCamera;
        if (cineVcam == null)
            return null;

        CinemachineBasicMultiChannelPerlin noise = cineVcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise == null) {
            noise = cineVcam.AddCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        return noise;
    }

    public void ScreenShakeImpulse(NoiseSetting noiseSetting, float targetStrength, float targetFrequency, float duration) {
        CinemachineBasicMultiChannelPerlin noise = getNoise();
        noise.enabled = true;
        noise.m_NoiseProfile = noiseSettingsDictionary[noiseSetting];

        // Immediately reset the amplitude and frequency
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;

        // Kill any active tweens on amplitude and frequency
        DOTween.Kill("AmplitudeGainTween");
        DOTween.Kill("FrequencyGainTween");

        // Create tweens with unique IDs
        DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, targetStrength, 0.5f)
            .SetId("AmplitudeGainTween");
        DOTween.To(() => noise.m_FrequencyGain, x => noise.m_FrequencyGain = x, targetFrequency, 0.5f)
            .SetId("FrequencyGainTween");

        // Start coroutine to remove noise effect after duration
        StartCoroutine(RemoveNoiseAfterDuration(noise, duration));
    }




    public void SetDefaultScreenShake(NoiseSetting noiseSetting, float targetAmplitude = 1f, float targetFrequency = 1f) {
        defaultScreenShakeSetting = noiseSetting;
        CinemachineBasicMultiChannelPerlin noise = getNoise();

        if (noiseSetting == NoiseSetting.None) {
            noise.enabled = false;

            DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, 0, 0.5f);
            DOTween.To(() => noise.m_FrequencyGain, x => noise.m_FrequencyGain = x, 0, 0.5f);
        } else {
            noise.enabled = true;
            noise.m_NoiseProfile = noiseSettingsDictionary[noiseSetting];

            DOTween.To(() => noise.m_AmplitudeGain, x => noise.m_AmplitudeGain = x, targetAmplitude, 0.5f);
            DOTween.To(() => noise.m_FrequencyGain, x => noise.m_FrequencyGain = x, targetFrequency, 0.5f);
        }
    }

    IEnumerator RemoveNoiseAfterDuration(CinemachineBasicMultiChannelPerlin noise, float duration) {
        yield return new WaitForSeconds(duration);
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
        noise.enabled = false;
        // Revert to the default screen shake after impulse is finished
        if (defaultScreenShakeSetting != NoiseSetting.None) {
            SetDefaultScreenShake(defaultScreenShakeSetting);
        }
    }
}

public enum NoiseSetting {
    ProfileSixdShake,
    ProfileSixdWobble,
    ProfileHandheldNormalMild,
    ProfileHandheldNormalStrong,
    ProfileHandheldNormalExtreme,
    ProfileHandheldTeleMild,
    ProfileHandheldTeleStrong,
    ProfileHandheldWideAngleMild,
    ProfileHandheldWideAngleStrong,
    None
}
