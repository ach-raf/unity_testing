using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] public InventoryScriptableObject inventory_scriptable_object;
    [SerializeField] public GameObject inventory_panel;
    [SerializeField] public GameObject item_slot_prefab;



    void OnEnable()
    {
        EventManager.InventoryItemAdded += RefreshInventory;

    }
    void OnDisable()
    {
        EventManager.InventoryItemAdded -= RefreshInventory;

    }
    public void OpenInventory()
    {
        inventory_panel.SetActive(true);
    }
    public void CloseInventory()
    {
        inventory_panel.SetActive(false);
    }
    public void AddItem(ItemScriptableObject item_scriptable_object)
    {
        GameObject item_slot = Instantiate(item_slot_prefab, inventory_panel.transform);
        item_slot.GetComponent<Item>().ItemScriptableObject = item_scriptable_object;
        item_slot.GetComponent<Item>().SetItem();
    }
    public void RefreshInventory()
    {
        foreach (Transform child in inventory_panel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemScriptableObject item_scriptable_object in inventory_scriptable_object.buildings_inventory.GetItems())
        {
            AddItem(item_scriptable_object);
        }
    }
}
