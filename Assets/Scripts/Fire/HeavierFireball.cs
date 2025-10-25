using UnityEngine;

public class HeavierFireball : BaseFireball
{
    public float extraGravity = 2f;
    void FixedUpdate() { if (_alive) Body.AddForce(Physics.gravity * extraGravity, ForceMode.Acceleration); }
}