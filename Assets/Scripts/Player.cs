using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Inventory buildings_inventory;
    private Inventory ressources_inventory;
    private PlayerControlls playerController;

    private void Awake()
    {
        playerController = new PlayerControlls();
    }
    void OnEnable()
    {
        playerController.Enable();
        playerController.Player.BuildingRotate.performed += BuildingRotatePerformed;
        playerController.Player.BuildingRotate.canceled += BuildingRotateCanceled;
    }
    void OnDisable()
    {
        playerController.Player.BuildingRotate.performed -= BuildingRotatePerformed;
        playerController.Player.BuildingRotate.canceled -= BuildingRotateCanceled;
        playerController.Disable();
    }

    // Update is called once per frame
    void LateUpdate()
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
    public void BuildingRotatePerformed(InputAction.CallbackContext context)
    {
        EventManager.OnBuildingRotatePerformed();
    }
    public void BuildingRotateCanceled(InputAction.CallbackContext context)
    {
        //EventManager.OnBuildingRotatePerformed();
    }
}
