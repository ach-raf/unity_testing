using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Inventory buildings_inventory;
    private Inventory ressources_inventory;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetBuildingsInventory(Inventory _inventory)
    {
        buildings_inventory = _inventory;
    }
    public Inventory GetBuildingsInventory()
    {
        return buildings_inventory;
    }

    public void SetRessourcesInventory(Inventory _inventory)
    {
        ressources_inventory = _inventory;
    }
    public Inventory GetRessourcesInventory()
    {
        return ressources_inventory;
    }
}
