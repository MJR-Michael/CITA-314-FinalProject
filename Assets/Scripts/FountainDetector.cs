using UnityEngine;

public class FountainDetector : MonoBehaviour
{
    [Header("Debug")]
    public bool debugOverride = false;

    private int coinCount = 0;

    public bool HasExactlyOneCoin()
    {
        // If debug override is enabled,
        // pretend this fountain has a coin
        if (debugOverride)
            return true;

        return coinCount == 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coinCount--;
        }
    }
}