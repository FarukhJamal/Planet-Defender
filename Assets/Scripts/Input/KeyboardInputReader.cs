using UnityEngine;

public class KeyboardInputReader : MonoBehaviour, IInputReader
{
    public float GetTurn() => Input.GetAxisRaw("Horizontal");
}