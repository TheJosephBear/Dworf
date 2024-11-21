using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowZone : MonoBehaviour, Ipickup {

    public float slowMultiplier = 0.5f;
    public float slowDuration = 0.5f;

    bool pickedUp = false;

    public void PickedUp(PlayerCharacter playerCharacter) {
        if (pickedUp) return;
        StartCoroutine(Effect(playerCharacter));
        pickedUp = true;
    }

    IEnumerator Effect(PlayerCharacter playerCharacter) {
        playerCharacter.GetSlowed(slowMultiplier, slowDuration);
        Destroy(gameObject);
        yield return null;
    }
}
