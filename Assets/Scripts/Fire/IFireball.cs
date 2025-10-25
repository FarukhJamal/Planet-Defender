using UnityEngine;

public interface IFireball
{
    Rigidbody Body { get; }
    void Init(Vector3 start, Vector3 target);
    void Nullify();
}