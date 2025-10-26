using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    [Header("Follow Target")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 15f, -5f);
    public float followSpeed = 5f;
    public float lookDownAngle = 60f; // tilt angle

    private void LateUpdate()
    {
        if (!target) return;

        // Desired camera position
        Vector3 desiredPos = target.position + offset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPos, followSpeed * Time.deltaTime);

        // Always look at the target with a top-down tilt
        Quaternion lookRot = Quaternion.Euler(lookDownAngle, 0f, 0f);
        transform.rotation = lookRot;
    }
}
