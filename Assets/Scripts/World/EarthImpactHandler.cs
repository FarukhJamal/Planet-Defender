using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EarthImpactHandler : MonoBehaviour
{
    void OnCollisionEnter(Collision c)
    {
        if (c.collider.CompareTag("Fireball"))
        {
            GameEvents.RaiseFireballHitEarth();
        }
    }
}