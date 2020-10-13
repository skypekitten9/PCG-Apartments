using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
    North,
    East,
    South,
    West
}

public class Furniture : MonoBehaviour
{
    public int sizeX, sizeZ;
    public Direction direction;
}
