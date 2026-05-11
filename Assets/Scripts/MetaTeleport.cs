using UnityEngine;

public class MetaTeleport : MonoBehaviour
{
    [Header("References")]
    public Transform playerRig;
    public Transform controller;

    [Header("Teleport Settings")]
    public float stickThreshold = 0.8f;
    public LayerMask groundLayer;

    private bool hasTeleported = false;

    void Update()
    {
        Vector2 stickInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (stickInput.y > stickThreshold && !hasTeleported)
        {
            TeleportToPoint();
            hasTeleported = true;
        }

        if (stickInput.y < 0.2f)
        {
            hasTeleported = false;
        }
    }

    void TeleportToPoint()
    {
        if (controller == null) return;

        Ray ray = new Ray(controller.position, controller.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 targetPosition = hit.point;

            // keep player level (optional depending on rig setup)
            targetPosition.y = playerRig.position.y;

            playerRig.position = targetPosition;
        }
    }
}