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
    private static int inventory_state = 0;



    private void Awake()
    {
        playerControlls = new PlayerControlls();

    }
    private void OnEnable()
    {
        playerControlls.Enable();

    }
    private void OnDisable()
    {
        playerControlls.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (playerControlls.Player.CloseBuildingInfo.triggered)
        {
            building_info_panel.SetActive(false);
        }

        if (playerControlls.Player.OpenInventory.triggered)
        {
            if (inventory_state % 2 == 0)
            {
                inventory_panel.SetActive(true);
                inventory_state++;
            }
            else
            {
                inventory_panel.SetActive(false);
                inventory_state++;
            }
        }
    }
}
