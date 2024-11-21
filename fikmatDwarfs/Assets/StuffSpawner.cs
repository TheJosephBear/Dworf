using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StuffSpawner : MonoBehaviour {

    public BoxCollider2D spawnArea;
    public List<GameObject> spawnableObjects;

    [SerializeField] private float spawnInterval = 1f; // Initial interval between spawns
    [SerializeField] private float minSpawnInterval = 0.2f; // Minimum interval between spawns
    [SerializeField] private float objectLifetime = 5f;
    public float difficultyGainSpeed = 0.05f;

    private BackgroundMoving backgroundMover;
    private float nextSpawnTime;
    private float ogSpawnInterval; // Store the original spawn interval
    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<float> spawnTimes = new List<float>();

    void Awake() {
        ogSpawnInterval = spawnInterval; // Store the original interval
        backgroundMover = FindAnyObjectByType<BackgroundMoving>();
        nextSpawnTime = Time.time + spawnInterval;
    }

    void Update() {
        if (!GamePlayLogic.Instance.gameStarted || GamePlayLogic.Instance.gameOver) {
            if (GamePlayLogic.Instance.gameOver) {
                ClearAllSpawnedObjects();
            }
            return;
        }

        // Adjust spawn interval based on the score
        AdjustSpawnIntervalBasedOnScore();

        if (Time.time >= nextSpawnTime) {
            SpawnRandomObject();
            nextSpawnTime = Time.time + spawnInterval;
        }

        MoveAndCleanupSpawnedObjects();
    }

    public void SpawnObjectToMoveWithWall(GameObject go, Vector3 position) {
        GameObject spawnedObject = Instantiate(go, position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

        spawnedObjects.Add(spawnedObject);
        spawnTimes.Add(Time.time);
    }

    void AdjustSpawnIntervalBasedOnScore() {
        // Get the player's score
        int score = FindAnyObjectByType<Stats>().GetHigherScore();

        // Calculate the spawn interval, gradually decreasing it as score increases
        spawnInterval = Mathf.Max(minSpawnInterval, ogSpawnInterval / (1 + score * difficultyGainSpeed));
    }

    void SpawnRandomObject() {
        if (spawnableObjects == null || spawnableObjects.Count == 0) return;

        GameObject objectToSpawn = spawnableObjects[Random.Range(0, spawnableObjects.Count)];
        Vector2 spawnPosition = GetRandomPositionInBox();

        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));

        spawnedObjects.Add(spawnedObject);
        spawnTimes.Add(Time.time);
    }

    Vector2 GetRandomPositionInBox() {
        Bounds bounds = spawnArea.bounds;
        float xPos = Random.Range(bounds.min.x, bounds.max.x);
        float yPos = bounds.min.y;
        return new Vector2(xPos, yPos);
    }

    void MoveAndCleanupSpawnedObjects() {
        float speed = backgroundMover != null ? backgroundMover.GetSpeed() : 1f;

        for (int i = spawnedObjects.Count - 1; i >= 0; i--) {
            GameObject obj = spawnedObjects[i];
            if (obj != null) {
                obj.transform.position += Vector3.up * speed * Time.deltaTime;

                if (Time.time - spawnTimes[i] > objectLifetime) {
                    Destroy(obj);
                    spawnedObjects.RemoveAt(i);
                    spawnTimes.RemoveAt(i);
                }
            }
        }
    }

    void ClearAllSpawnedObjects() {
        foreach (GameObject obj in spawnedObjects) {
            if (obj != null) {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
        spawnTimes.Clear();
    }
}
