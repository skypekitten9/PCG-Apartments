using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FamilyType { Dining, Parlour, None }
public class Furniture : MonoBehaviour
{
    public int width, height;
    public FamilyType familyType;
}
