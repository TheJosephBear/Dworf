using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSprite : MonoBehaviour {

    public AnimatorOverrideController animationSpecial;

    void OnEnable() {
        SpecialAnimationStart();
    }

    public void SpecialAnimationStart() {
        if (animationSpecial != null) {
            GetComponent<Animator>().runtimeAnimatorController = animationSpecial;
        }
    }

}