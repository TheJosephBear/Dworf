using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceBomb : MonoBehaviour {

    public GameObject ExplosionEffect;
    public float speed = 1f;
    public float explosionForce = 1000f;    // The strength of the explosion
    public float explosionRadius = 5f;     // The radius of the explosion
    public LayerMask affectedLayers;       // Layers to be affected by the explosion

    Rigidbody2D rb;
    Vector2 direction;

    void Awake() {
        rb = GetComponent<Rigidbody2D>();
        // Default direction is down
        direction = new Vector2(0, -1);
    }

    void Update() {
        Move();
    }

    public void LightTheFuse(float timeBeforeBOOM, Vector2 moveDirection) {
        direction = moveDirection;
        StartCoroutine(BombCoroutine(timeBeforeBOOM));
    }

    IEnumerator BombCoroutine(float timeBeforeBOOM) {
        yield return new WaitForSeconds(timeBeforeBOOM);
        Explode();
    }

    void Explode() {
        // Detect all colliders within the explosion radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, affectedLayers);

        foreach (Collider2D collider in colliders) {
            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>(); // Use Rigidbody2D for 2D physics

            print("I hit something: " + collider.name); // Debugging output

            // Apply force if Rigidbody2D is present
            if (rb != null) {
                print("it has rigid body and im trying to move it ");
                Vector2 direction = (rb.position - (Vector2)transform.position).normalized;
                rb.AddForce(direction * explosionForce, ForceMode2D.Impulse);
            }
        }

        // Instantiate visual explosion effect
        Instantiate(ExplosionEffect, transform.position, Quaternion.identity);

        // Destroy the explosive object
        Destroy(gameObject);
    }

    void Move() {
        rb.velocity = direction * speed;
        if (direction == Vector2.zero) {
            rb.velocity = Vector2.zero;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
