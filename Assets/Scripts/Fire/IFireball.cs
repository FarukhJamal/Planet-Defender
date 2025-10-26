using UnityEngine;

public interface IFireball
{
    Rigidbody Body { get; }

    /// <summary>
    /// Initializes the fireball and sets its trajectory toward the given Earth.
    /// </summary>
    void Init(Transform earthTransform);

    /// <summary>
    /// Called when the fireball is nullified (encapsulated by the snake).
    /// </summary>
    void Nullify();
}
