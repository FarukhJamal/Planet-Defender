public interface IInputReader
{
    // Returns desired lateral input [-1, 1] along local tangent (left/right)
    float GetTurn();

    // Returns desired vertical input [-1, 1] (up/down movement)
    float GetVertical();
}
