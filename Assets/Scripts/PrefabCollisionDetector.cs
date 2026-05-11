using UnityEngine;

public class PrefabTriggerDetector : MonoBehaviour
{
    public GameObject objectToEnable;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Target"))
        {
            objectToEnable.SetActive(true);
            hasTriggered = true;

            // Stop further checks
            enabled = false;
        }
    }
}