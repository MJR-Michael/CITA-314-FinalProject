using UnityEngine;

public class FishingRodCast : MonoBehaviour
{
    [Header("Rod Setup")]
    public Transform rodTip;
    public float castRange = 30f;

    [Header("Water Detection")]
    public LayerMask waterLayer;

    [Header("Bobber")]
    public GameObject bobberPrefab;
    public float bobberSpawnOffset = 0.1f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip correctCastSound;
    public AudioClip incorrectCastSound;

    private GameObject currentBobber;

    void Update()
    {
        // A button (right controller)
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.RTouch))
        {
            TryCastLine();
        }
    }

    void TryCastLine()
    {
        Ray ray = new Ray(rodTip.position, rodTip.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, castRange, waterLayer))
        {
            if (hit.collider.CompareTag("Water") || hit.collider.name == "Water")
            {
                PlaySound(correctCastSound);
                CastLine(hit.point, hit.normal);
            }
            else
            {
                PlaySound(incorrectCastSound);
            }
        }
        else
        {
            PlaySound(incorrectCastSound);
        }
    }

    void CastLine(Vector3 hitPoint, Vector3 hitNormal)
    {
        // Remove old bobber
        if (currentBobber != null)
        {
            Destroy(currentBobber);
        }

        Vector3 spawnPos = hitPoint + hitNormal * bobberSpawnOffset;

        currentBobber = Instantiate(
            bobberPrefab,
            spawnPos,
            Quaternion.identity
        );

        // Pass rod reference to bobber
        BobberFishSpawner bobberScript =
            currentBobber.GetComponent<BobberFishSpawner>();

        if (bobberScript != null)
        {
            bobberScript.rodTip = rodTip;
        }

        Debug.Log("🎣 Bobber spawned at: " + spawnPos);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}