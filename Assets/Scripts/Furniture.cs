using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FamilyType { Surrounding, Directional, None }
public enum FurnitureType { Chair, Table, TV, Lamp, Sofa, None }
public class Furniture : MonoBehaviour
{
    public int width, height;
    public FamilyType familyType;
    public FurnitureType furnitureType;
    public FurnitureType likes;
    public int prefferedDistance;
}
