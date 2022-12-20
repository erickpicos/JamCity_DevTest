using UnityEngine;

public class Floor : MonoBehaviour
{
    public FloorType Type;
}

public enum FloorType
{
    None,
    Wood,
    Metal,
    Grass
}