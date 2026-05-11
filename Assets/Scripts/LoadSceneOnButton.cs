using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnYButton : MonoBehaviour
{
    [SerializeField] private string sceneName = "YourSceneName";

    void Update()
    {
        // Y button = Left controller secondary button
        if (OVRInput.GetDown(OVRInput.Button.Four))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}