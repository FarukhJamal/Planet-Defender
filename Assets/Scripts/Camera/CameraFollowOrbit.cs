using UnityEngine;

public class CameraFollowOrbit : MonoBehaviour
{
    [Header("References")]
    public Transform earth;       // target (Earth)
    public Transform snakeHead;   // the snake to follow

    [Header("Camera Settings")]
    public float distance = 30f;  // how far from Earth
    public float heightOffset = 5f; // slight elevation above orbit
    public float followSmooth = 2.5f; // camera smoothing speed
    public float lookSmooth = 4f;     // smooth lookAt speed

    [Header("Rotation Control")]
    public float followAngleLerp = 0.3f; // how much camera aligns with snake heading
    public float rotationLag = 0.15f;    // lag effect for cinematic feel

    private Vector3 _desiredPosition;
    private Quaternion _desiredRotation;
    private float _currentYaw;

    void LateUpdate()
    {
        if (!earth || !snakeHead) return;

        // --- 1) Find snake direction around Earth ---
        Vector3 toHead = (snakeHead.position - earth.position).normalized;

        // compute tangent (snake’s travel direction)
        Vector3 forward = snakeHead.forward;
        Vector3 right = Vector3.Cross(forward, toHead).normalized;

        // derive the current yaw angle around Earth (so camera follows orbit path)
        Vector3 camDir = Vector3.ProjectOnPlane(-forward, toHead).normalized;

        // target position: opposite the snake’s travel direction, offset by height
        _desiredPosition = earth.position + camDir * distance + toHead * heightOffset;

        // smooth position transition
        transform.position = Vector3.Lerp(transform.position, _desiredPosition, followSmooth * Time.deltaTime);

        // desired rotation: look at Earth, but biased toward snake direction
        Vector3 lookTarget = Vector3.Lerp(earth.position, snakeHead.position, followAngleLerp);
        Quaternion targetRot = Quaternion.LookRotation(lookTarget - transform.position, toHead);
        _desiredRotation = Quaternion.Slerp(transform.rotation, targetRot, lookSmooth * Time.deltaTime);

        transform.rotation = _desiredRotation;
    }

    void OnDrawGizmosSelected()
    {
        if (earth)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(earth.position, distance);
        }
    }
}
