using UnityEngine;

public class DeleteOnClick : MonoBehaviour
{
    public GameObject targetObject;

    public void DeleteTarget()
    {
        if (targetObject != null)
        {
            Destroy(targetObject);
        }
    }
}