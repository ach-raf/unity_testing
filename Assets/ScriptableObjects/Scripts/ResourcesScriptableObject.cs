using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Resource SO", menuName = "Resources/Resource SO", order = 4)]

public class ResourcesScriptableObject : ItemScriptableObject
{
    private int max_quantity;
    private bool stackable;
}
