using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMoving : MonoBehaviour {
    public List<Transform> backgrounds = new List<Transform>(); // List of background transforms
    [SerializeField]
    private float speed = 1f;
    private bool isMoving = false;
    private float backgroundHeight;

    void Start() {
        if (backgrounds.Count == 0) {
            Debug.LogError("No backgrounds assigned!");
            return;
        }

        // Calculate the height of the backgrounds assuming they all have the same size
        backgroundHeight = backgrounds[0].GetComponent<SpriteRenderer>().bounds.size.y;
    }

    void Update() {
        if (isMoving) {
            MoveBackgrounds();
        }
    }

    public void ToggleMovement(bool turnOn) {
        isMoving = turnOn;
    }

    public void SetSpeed(float newSpeed) {
        speed = newSpeed;
    }

    public float GetSpeed() {
        return speed;
    }

    void MoveBackgrounds() {
        // Move each background upward
        foreach (Transform background in backgrounds) {
            background.position += Vector3.up * speed * Time.deltaTime;
        }

        // Check if the first background has moved off-screen
        if (backgrounds[0].position.y >= backgroundHeight) {
            // Reposition the first background below the last one
            Transform firstBackground = backgrounds[0];
            Transform lastBackground = backgrounds[backgrounds.Count - 1];
            firstBackground.position = new Vector3(firstBackground.position.x, lastBackground.position.y - backgroundHeight, firstBackground.position.z);

            // Move the first background to the end of the list
            backgrounds.RemoveAt(0);
            backgrounds.Add(firstBackground);
        }
    }
}
