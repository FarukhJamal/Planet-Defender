using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public abstract class BaseFireball : MonoBehaviour, IFireball
{
    public Rigidbody Body { get; private set; }
    protected bool _alive = true;
    protected Transform earth;

    [Header("Fireball Settings")]
    public float minSpeed = 6f;
    public float maxSpeed = 10f;

    protected virtual void Awake()
    {
        Body = GetComponent<Rigidbody>();

        var col = GetComponent<SphereCollider>();
        col.material = new PhysicMaterial
        {
            bounciness = 0.6f,
            bounceCombine = PhysicMaterialCombine.Maximum
        };
        col.isTrigger = false;
        gameObject.tag = "Fireball";
        Body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    /// <summary>
    /// Initialize fireball towards Earth's center.
    /// </summary>
    public virtual void Init(Transform earthTransform)
    {
        earth = earthTransform;
        _alive = true;

        // Reset rigidbody
        Body.velocity = Vector3.zero;
        Body.angularVelocity = Vector3.zero;

        // Compute direction toward Earth
        Vector3 dir = (earth.position - transform.position).normalized;

        // Assign velocity directly toward Earth
        float speed = Random.Range(minSpeed, maxSpeed);
        Body.velocity = dir * speed;
    }

    /// <summary>
    /// Called when encapsulated by snake.
    /// </summary>
    public virtual void Nullify()
    {
        if (!_alive) return;
        _alive = false;

        GameEvents.RaiseFireballNullified();
        GameEvents.RaiseSegmentGained(); // snake grows when nullified

        Destroy(gameObject);
    }


    /// <summary>
    /// Called when hitting Earth or after nullification — returns to pool.
    /// </summary>
    protected virtual void Deactivate()
    {
        Body.velocity = Vector3.zero;
        Body.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (!_alive) return;

        if (collision.collider.CompareTag("Earth"))
        {
            _alive = false;
            GameEvents.RaiseFireballHitEarth();
            Deactivate();
        }
    }
}
