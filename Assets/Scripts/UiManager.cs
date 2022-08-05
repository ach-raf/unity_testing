using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UiManager : MonoBehaviour
{
    [SerializeField] public GameObject canvas;
    [SerializeField] public GameObject inventory_panel;
    [SerializeField] public GameObject building_info_panel;
    private PlayerControlls playerControlls;
    //private static int inventory_state = 0;
    private bool inventory_state = true;



    private void Awake()
    {
        playerControlls = new PlayerControlls();

    }
    private void OnEnable()
    {
        playerControlls.Enable();
        EventManager.InventoryItemClicked += InventoryItemClicked;


    }
    private void OnDisable()
    {
        playerControlls.Disable();
        EventManager.InventoryItemClicked -= InventoryItemClicked;

    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerControlls.Player.AddResource.triggered)
        {
            EventManager.OnDebugResourceAdded();
            //StartCoroutine(buildable.GetBuildingData().StartProcessingResources());
        }
        if (playerControlls.Player.CloseBuildingInfo.triggered)
        {
            building_info_panel.SetActive(false);
        }

        if (playerControlls.Player.OpenInventory.triggered)
        {

            inventory_panel.SetActive(inventory_state);
            inventory_state = !inventory_state;

        }
    }
    public void InventoryItemClicked(ItemScriptableObject item_scriptable_object)
    {
        inventory_panel.SetActive(inventory_state);
        inventory_state = !inventory_state;


    }
}
