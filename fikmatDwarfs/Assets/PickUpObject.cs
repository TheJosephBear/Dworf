using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            GetComponent<Ipickup>().PickedUp(collision.GetComponent<PlayerCharacter>().GetPlayer());
        }
    }
}
