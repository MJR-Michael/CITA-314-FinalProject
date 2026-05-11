using UnityEngine;

public class SpawnObjectOnButton : MonoBehaviour
{
    [Header("Prefab to spawn")]
    public GameObject prefabToSpawn;

    [Header("Ray settings")]
    public float maxDistance = 10f;
    public LayerMask hitLayers = ~0;

    void Update()
    {
        // X button on left Oculus controller
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            SpawnAtControllerAim();
        }
    }

    void SpawnAtControllerAim()
    {
        if (prefabToSpawn == null)
        {
            Debug.LogWarning("Prefab not assigned!");
            return;
        }

        // Get left controller position and rotation
        Vector3 origin = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
        Quaternion rotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch);

        Vector3 direction = rotation * Vector3.forward;

        Vector3 spawnPosition;

        // Raycast from controller forward
        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, hitLayers))
        {
            spawnPosition = hit.point;
        }
        else
        {
            // fallback if nothing is hit
            spawnPosition = origin + direction * 2f;
        }

        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}