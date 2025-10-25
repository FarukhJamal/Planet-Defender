using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target; // Earth
    public float distance = 25f;
    public float orbitSpeed = 30f; // degrees/sec auto-orbit for parallax
    public Vector3 offset = Vector3.up * 5f; // slight tilt

    void LateUpdate()
    {
        if (!target) return;
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        transform.position = (transform.position - target.position).normalized * distance + target.position + offset;
        transform.LookAt(target.position);
    }
}