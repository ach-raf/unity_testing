using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item SO", menuName = "Items/Item SO", order = 2)]
public class ItemScriptableObject : ScriptableObject
{
    public int id;
    public new string name;
    public GameObject gameObject;
    public Sprite ItemImage;
    public int quantity;
    //private int max_quantity;
    //private bool stackable;
}
