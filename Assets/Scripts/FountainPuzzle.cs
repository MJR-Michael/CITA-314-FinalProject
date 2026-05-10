using UnityEngine;

public class FountainPuzzle : MonoBehaviour
{
    public FountainDetector fountain1;
    public FountainDetector fountain2;

    public GameObject objectToSpawn;
    public Transform spawnPoint;

    private bool hasSpawned = false;

    void Update()
    {
        if (!hasSpawned &&
            fountain1.HasExactlyOneCoin() &&
            fountain2.HasExactlyOneCoin())
        {
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        Instantiate(
            objectToSpawn,
            spawnPoint.position,
            spawnPoint.rotation,
            spawnPoint
        );

        hasSpawned = true;
    }
}