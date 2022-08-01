using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory inventory;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
    }
    public Inventory GetInventory()
    {
        return inventory;
    }
}
