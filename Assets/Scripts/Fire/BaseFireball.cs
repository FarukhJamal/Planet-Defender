using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public abstract class BaseFireball : MonoBehaviour, IFireball
{
    public Rigidbody Body { get; private set; }
    protected bool _alive = true;

    protected virtual void Awake()
    {
        Body = GetComponent<Rigidbody>();
        var col = GetComponent<SphereCollider>();
        col.material = new PhysicMaterial { bounciness = 0.6f, bounceCombine = PhysicMaterialCombine.Maximum };
        col.isTrigger = false;
        gameObject.tag = "Fireball";
    }

    public virtual void Init(Vector3 start, Vector3 target)
    {
        transform.position = start;
        Vector3 dir = (target - start).normalized;
        Body.velocity = dir * Random.Range(6f, 10f);
    }

    public virtual void Nullify()
    {
        if (!_alive) return; _alive = false;
        GameEvents.RaiseFireballNullified();
        Destroy(gameObject);
    }
}