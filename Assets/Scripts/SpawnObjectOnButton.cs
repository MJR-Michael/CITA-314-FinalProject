using UnityEngine;

public class SpawnObjectOnButton : MonoBehaviour
{
    [Header("Prefab")]
    public GameObject prefabToSpawn;

    [Header("OVR References")]
    public Transform leftControllerAnchor; // Assign OVRCameraRig LeftHandAnchor

    [Header("Settings")]
    public float maxDistance = 10f;
    public LayerMask hitLayers = ~0;

    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.One, OVRInput.Controller.LTouch))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (prefabToSpawn == null || leftControllerAnchor == null)
        {
            Debug.LogWarning("Missing prefab or controller anchor reference!");
            return;
        }

        Vector3 origin = leftControllerAnchor.position;
        Vector3 direction = leftControllerAnchor.forward;

        Vector3 spawnPosition;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance, hitLayers))
        {
            spawnPosition = hit.point;
        }
        else
        {
            spawnPosition = origin + direction * 2f;
        }

        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
    }
}