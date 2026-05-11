using UnityEngine;
using System.Collections;

public class BobberFishSpawner : MonoBehaviour
{
    [Header("Timing")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 10f;

    [Header("Fish Pool")]
    public GameObject[] fishPrefabs;

    [Header("References")]
    public Transform rodTip;

    [Header("Spawn")]
    public Transform fishSpawnPoint;

    [Header("Reeling")]
    public float reelSpeed = 2f;

    private bool isOnWater = false;
    private Coroutine fishRoutine;

    private Transform currentFish;

    void Update()
    {
        ReelFish();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isOnWater = true;
            StartFishingCycle();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            isOnWater = false;
            StopFishingCycle();
        }
    }

    void StartFishingCycle()
    {
        if (fishRoutine == null)
        {
            fishRoutine = StartCoroutine(FishSpawnRoutine());
        }
    }

    void StopFishingCycle()
    {
        if (fishRoutine != null)
        {
            StopCoroutine(fishRoutine);
            fishRoutine = null;
        }
    }

    IEnumerator FishSpawnRoutine()
    {
        while (isOnWater)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);

            yield return new WaitForSeconds(waitTime);

            if (!isOnWater)
                yield break;

            SpawnRandomFish();

            // One fish per cast
            yield break;
        }
    }

    void SpawnRandomFish()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("No fish prefabs assigned!");
            return;
        }

        int randomIndex = Random.Range(0, fishPrefabs.Length);

        GameObject fishPrefab = fishPrefabs[randomIndex];

        Vector3 spawnPos =
            fishSpawnPoint != null
            ? fishSpawnPoint.position
            : transform.position;

        GameObject spawnedFish = Instantiate(
            fishPrefab,
            spawnPos,
            Quaternion.identity
        );

        // Store fish reference for reeling
        currentFish = spawnedFish.transform;

        Debug.Log("Spawned fish: " + fishPrefab.name);
    }

    void ReelFish()
    {
        // Hold B button on right Meta controller
        bool isReeling =
            OVRInput.Get(
                OVRInput.Button.Two,
                OVRInput.Controller.RTouch
            );

        if (isReeling && currentFish != null && rodTip != null)
        {
            currentFish.position = Vector3.MoveTowards(
                currentFish.position,
                rodTip.position,
                reelSpeed * Time.deltaTime
            );
        }
    }
}