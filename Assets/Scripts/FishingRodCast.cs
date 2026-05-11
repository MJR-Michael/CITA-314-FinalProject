using System.Collections;
using UnityEngine;

public class FishingRodCast : MonoBehaviour
{
    [Header("References")]
    public Transform rodTip;
    public LineRenderer lineRenderer;
    public AudioSource audioSource;

    [Header("Fish Prefabs (3 types)")]
    public GameObject[] fishPrefabs;

    [Header("Audio")]
    public AudioClip invalidCastSFX;

    [Header("Settings")]
    public float maxCastDistance = 20f;
    public LayerMask waterLayer;

    [Header("State")]
    public bool isCast = false;
    public bool fishHooked = false;

    private Vector3 hookPoint;
    private GameObject currentFish;

    void Start()
    {
        if (lineRenderer != null)
            lineRenderer.positionCount = 2;
    }

    void Update()
    {
        HandleInput();
        UpdateLine();
    }

    void HandleInput()
    {
        if (OVRInput.GetDown(OVRInput.Button.One)) // A button
        {
            TryCast();
        }
    }

    void TryCast()
    {
        if (isCast) return;

        Vector3 origin = rodTip.position;
        Vector3 direction = rodTip.forward;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxCastDistance, waterLayer))
        {
            hookPoint = hit.point;
            isCast = true;

            Debug.Log("🎣 Cast successful!");

            OnCastSuccess();
        }
        else
        {
            Debug.Log("❌ Invalid cast - not aiming at water");

            PlayInvalidCastFeedback();
        }
    }

    void PlayInvalidCastFeedback()
    {
        // 🔊 Sound effect
        if (audioSource != null && invalidCastSFX != null)
        {
            audioSource.PlayOneShot(invalidCastSFX);
        }

        // 🎮 Haptic feedback
        OVRInput.SetControllerVibration(0.2f, 0.3f, OVRInput.Controller.RTouch);
    }

    void OnCastSuccess()
    {
        StartCoroutine(FishBiteRoutine());
    }

    IEnumerator FishBiteRoutine()
    {
        float waitTime = Random.Range(5f, 10f);

        Debug.Log($"🐟 Waiting for fish bite: {waitTime}s");

        yield return new WaitForSeconds(waitTime);

        SpawnFishAtHook();
    }

    void SpawnFishAtHook()
    {
        if (fishPrefabs == null || fishPrefabs.Length == 0)
        {
            Debug.LogWarning("No fish prefabs assigned!");
            return;
        }

        int index = Random.Range(0, fishPrefabs.Length);
        GameObject fishPrefab = fishPrefabs[index];

        currentFish = Instantiate(fishPrefab, hookPoint, Quaternion.identity);

        // Attach fish to line (temporary system)
        currentFish.transform.SetParent(lineRenderer.transform);

        fishHooked = true;

        Debug.Log("🐟 Fish hooked!");
    }

    void UpdateLine()
    {
        if (!lineRenderer || !rodTip) return;

        lineRenderer.SetPosition(0, rodTip.position);

        if (isCast)
        {
            lineRenderer.SetPosition(1, hookPoint);
        }
        else
        {
            // Preview cast direction
            Vector3 origin = rodTip.position;
            Vector3 direction = rodTip.forward;

            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxCastDistance, waterLayer))
            {
                lineRenderer.SetPosition(1, hit.point);
            }
            else
            {
                lineRenderer.SetPosition(1, origin + direction * maxCastDistance);
            }
        }
    }

    // Optional reset for testing
    public void ResetCast()
    {
        isCast = false;
        fishHooked = false;
        hookPoint = rodTip.position;

        if (currentFish != null)
            Destroy(currentFish);
    }
}