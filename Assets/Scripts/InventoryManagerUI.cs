using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManagerUI : MonoBehaviour
{
    //[SerializeField] public InventoryScriptableObject inventory_scriptable_object;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject building_info_panel;
    [SerializeField] public GameObject inventory_panel;
    [SerializeField] public GameObject item_slot_prefab;
    [SerializeField] public GameObject building_info;
    public BuildingScriptableObject[] items_objects;
    private Button item_button;
    private Player player_component;
    private Buildable selected_buildable;


    void OnEnable()
    {
        EventManager.RefreshBuildingPanel += RefreshBuildingPanel;
        EventManager.InventoryItemAdded += RefreshInventory;

        EventManager.BuildableClicked += BuildableClicked;
        EventManager.DebugResourceAdded += DebugResourceAdded;



    }
    void OnDisable()
    {
        EventManager.RefreshBuildingPanel -= RefreshBuildingPanel;
        EventManager.InventoryItemAdded -= RefreshInventory;


        EventManager.BuildableClicked -= BuildableClicked;

        EventManager.DebugResourceAdded -= DebugResourceAdded;



    }
    private void Awake()
    {
        player_component = player.GetComponent<Player>();
        player_component.SetBuildingsInventory(new Inventory());
    }
    // Start is called before the first frame update
    void Start()
    {

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
    public void RefreshInventory(ItemScriptableObject building_data)
    {
        Debug.Log("RefreshInventory");
        foreach (Transform child in inventory_panel.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (BuildingScriptableObject item_scriptable_object in player_component.GetBuildingsInventory().GetItems())
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
    public ItemScriptableObject GetItem(BuildingScriptableObject item_scriptable_object)
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
        Debug.Log(inventory_panel.transform.GetComponent<BuildingScriptableObject>().name);
        return inventory_panel.transform.GetComponent<BuildingScriptableObject>();
    }
    public void PrePopulateInventory()
    {
        foreach (BuildingScriptableObject item_scriptable_object in items_objects)
        {
            player_component.GetBuildingsInventory().AddItem(item_scriptable_object);

        }
    }

    public void BuildableClicked(Buildable buildable)
    {
        selected_buildable = buildable;
        building_info_panel.transform.position = Camera.main.ScreenToViewportPoint(buildable.transform.position);
        building_info_panel.SetActive(true);
        //building_info.GetComponent<BuildingScriptableObject>()..SetBuildingData(buildable.GetBuildingData());

        foreach (Transform child in building_info.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemScriptableObject item_scriptable_object in buildable.GetBuildingData().GetInventory().GetItems())
        {
            GameObject item_slot = Instantiate(item_slot_prefab, building_info.transform);
            item_slot.GetComponent<Item>().ItemScriptableObject = item_scriptable_object;
            item_slot.GetComponent<Item>().SetItem();
        }
    }



    public void DebugResourceAdded()
    {
        StartCoroutine(selected_buildable.GetBuildingData().StartProcessingResources());
        //BuildableClicked(selected_buildable);
    }
    public void RefreshBuildingPanel()
    {
        Debug.Log("RefreshBuildingInventoryPanel");
        foreach (Transform child in building_info.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemScriptableObject item_scriptable_object in selected_buildable.GetBuildingData().GetInventory().GetItems())
        {
            GameObject item_slot = Instantiate(item_slot_prefab, building_info.transform);
            item_slot.GetComponent<Item>().ItemScriptableObject = item_scriptable_object;
            item_slot.GetComponent<Item>().SetItem();
        }
    }



}
