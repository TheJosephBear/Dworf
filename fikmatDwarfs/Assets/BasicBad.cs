using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicBad : MonoBehaviour, Ipickup {

    public GameObject particleEffect;
    public float screenShakeStrength = 10f;
    bool pickedUp = false;
    public void PickedUp(Player player) {
        if (pickedUp) return;
        StartCoroutine(DeathEffect(player));
        pickedUp = true;
    }

    IEnumerator DeathEffect(Player player) {
        AudioManager.Instance.PlaySound(SoundType.BOMBA);
        Instantiate(particleEffect, transform.position, Quaternion.identity);
        ScreenEffectManager.Instance.ScreenShakeImpulse(NoiseSetting.ProfileSixdShake, screenShakeStrength, screenShakeStrength, 0.2f);
        Destroy(gameObject);


        //   player.Die();
        yield return null;
        // Start slow-motion effect
        //  Time.timeScale = 0.1f;
        /*
        if ((!GamePlayLogic.Instance.PlayerOne.isDead && GamePlayLogic.Instance.PlayerTwo.isDead) || (GamePlayLogic.Instance.PlayerOne.isDead && !GamePlayLogic.Instance.PlayerTwo.isDead)) {
            // Fade out using real-time unaffected by time scale
            Fader.Instance.Fade(true, 0.2f, Color.white);
            yield return new WaitForSecondsRealtime(0.5f);  // Wait in real-time for fade effect

            Fader.Instance.Fade(false, 0.1f, Color.white);
            yield return new WaitForSecondsRealtime(0.1f);  // Wait in real-time for fade-in effect
        }
        // Reset time scale back to normal
        Time.timeScale = 1f;

        // Spawn particle effect
        Instantiate(particleEffect, transform.position, Quaternion.identity);

        // Screen shake in real-time to keep timing unaffected by time scale
        ScreenEffectManager.Instance.ScreenShakeImpulse(NoiseSetting.ProfileSixdShake, screenShakeStrength, screenShakeStrength, 0.2f);
        */
    }
}
