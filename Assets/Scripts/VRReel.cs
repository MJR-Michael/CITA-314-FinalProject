using UnityEngine;

public class VRReel : MonoBehaviour
{
    [Header("Reel Tracking")]
    public Transform reelTransform;

    [Header("Fishing Target")]
    public Transform bobberOrFish;

    [Header("Reeling Settings")]
    public float reelStrength = 2f;   // how fast fish comes in
    public float rotationMultiplier = 10f;

    private float lastZRotation;
    private float reelSpeed;

    void Start()
    {
        lastZRotation = reelTransform.localEulerAngles.z;
    }

    void Update()
    {
        TrackReelRotation();
        PullFish();
    }

    void TrackReelRotation()
    {
        float currentZ = reelTransform.localEulerAngles.z;

        float deltaZ = Mathf.DeltaAngle(lastZRotation, currentZ);

        // Convert rotation speed into reel speed
        reelSpeed = deltaZ * rotationMultiplier;

        lastZRotation = currentZ;
    }

    void PullFish()
    {
        if (bobberOrFish == null) return;

        // Move fish toward reel (or rod position instead if you prefer)
        Vector3 direction = (transform.position - bobberOrFish.position).normalized;

        float moveAmount = reelSpeed * reelStrength * Time.deltaTime;

        bobberOrFish.position += direction * moveAmount;
    }
}