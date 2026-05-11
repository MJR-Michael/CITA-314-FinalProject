using UnityEngine;

public class FountainPuzzle : MonoBehaviour
{
    public FountainDetector fountain1;
    public FountainDetector fountain2;

    public GameObject[] objectsToEnable;

    private bool hasActivated = false;

    void Update()
    {
        if (!hasActivated &&
            fountain1.HasExactlyOneCoin() &&
            fountain2.HasExactlyOneCoin())
        {
            EnableObjects();
        }
    }

    void EnableObjects()
    {
        foreach (GameObject obj in objectsToEnable)
        {
            if (obj != null)
                obj.SetActive(true);
        }

        hasActivated = true;
    }
}