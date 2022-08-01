using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    [SerializeField] public InventoryScriptableObject inventory_scriptable_object;
    [SerializeField] public GameObject inventory_panel;
    [SerializeField] public GameObject item_slot_prefab;
    public BuildingScriptableObject[] items_objects;
    private Button item_button;





    void OnEnable()
    {
        EventManager.InventoryItemAdded += RefreshInventory;

    }
    void OnDisable()
    {
        EventManager.InventoryItemAdded -= RefreshInventory;

    }
    private void Awake()
    {
        inventory_scriptable_object.buildings_inventory = new Inventory();
    }
    // Start is called before the first frame update
    void Start()
    {
        item_button = item_slot_prefab.GetComponent<Button>();
        item_button.onClick.AddListener(() => { GetItemInfo(); });
        //inventory_scriptable_object.buildings_inventory.AddItem(items_objects[0]);

        //AddItem(items_objects[0]);
        PrePopulateInventory();
    }
    public void OpenInventory()
    {
        inventory_panel.SetActive(true);
    }
    public void CloseInventory()
    {
        inventory_panel.SetActive(false);
    }
    public void AddItem(BuildingScriptableObject item_scriptable_object)
    {
        GameObject item_slot = Instantiate(item_slot_prefab, inventory_panel.transform);
        item_slot.GetComponent<Item>().ItemScriptableObject = item_scriptable_object;
        item_slot.GetComponent<Item>().SetItem();
        //inventory_scriptable_object.buildings_inventory.AddItem(item_scriptable_object);

    }
    public void RefreshInventory()
    {
        foreach (Transform child in inventory_panel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (BuildingScriptableObject item_scriptable_object in inventory_scriptable_object.buildings_inventory.GetItems())
        {
            AddItem(item_scriptable_object);
        }
    }
    public void RemoveItem(BuildingScriptableObject item_scriptable_object)
    {
        foreach (Transform child in inventory_panel.transform)
        {
            if (child.GetComponent<Item>().ItemScriptableObject == item_scriptable_object)
            {
                Destroy(child.gameObject);
            }
        }
    }
    public BuildingScriptableObject GetItem(BuildingScriptableObject item_scriptable_object)
    {
        foreach (Transform child in inventory_panel.transform)
        {
            if (child.GetComponent<Item>().ItemScriptableObject == item_scriptable_object)
            {
                return child.GetComponent<Item>().ItemScriptableObject;
            }
        }
        return null;
    }
    public BuildingScriptableObject GetItemInfo()
    {
        Debug.Log(inventory_panel.transform.GetComponent<Item>().ItemScriptableObject.name);
        return inventory_panel.transform.GetComponent<Item>().ItemScriptableObject;
    }
    public void PrePopulateInventory()
    {
        foreach (BuildingScriptableObject item_scriptable_object in items_objects)
        {
            inventory_scriptable_object.buildings_inventory.AddItem(item_scriptable_object);

        }
    }


}
