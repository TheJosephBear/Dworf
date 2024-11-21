using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGood : MonoBehaviour, Ipickup {
    public int ScoreAmountGained = 10;
    public GameObject particleEffect;
    public float screenShakeStrength = 5f;

    bool pickedUp = false;

    public void PickedUp(PlayerCharacter playerCharacter) {
        if (pickedUp) return;
        AudioManager.Instance.PlaySound(SoundType.click_scifi);
        playerCharacter.GetPlayer().IncreaseScore(ScoreAmountGained);
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        ScreenEffectManager.Instance.ScreenShakeImpulse(NoiseSetting.ProfileSixdShake, screenShakeStrength, screenShakeStrength, 0.2f);
        Destroy(gameObject);
        pickedUp = true;
    }
}
