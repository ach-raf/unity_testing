using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public BuildingScriptableObject[] building_objects;
    public ItemScriptableObject[] items_objects;

    [SerializeField] public GameObject canvas;
    [SerializeField] public InventoryScriptableObject inventory_scriptable_object;
    [SerializeField] public GameObject inventory_panel;
    [SerializeField] public Camera current_camera;
    [SerializeField] LayerMask ground_mask;
    [SerializeField] GameObject buildings_holder;

    public PlayerControlls playerControlls;
    public GridScriptableObject grid_scriptable_object;
    public BuildingScriptableObject selected_building_object;
    public Material preview_material;
    private GameObject preview_object = null;
    private static int inventory_state = 0;
    private static int selected_building_index = 0;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
        inventory_scriptable_object.buildings_inventory = new Inventory();
    }

    private void OnEnable()
    {
        playerControlls.Enable();

    }
    private void OnDisable()
    {
        playerControlls.Disable();
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (preview_object)
        {
            Vector3 preview_position = new Vector3(MouseOperations.GroundMousePosition(current_camera, ground_mask).point.x, MouseOperations.GroundMousePosition(current_camera, ground_mask).point.y, MouseOperations.GroundMousePosition(current_camera, ground_mask).point.z);
            preview_object.transform.position = preview_position;
        }
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
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
        if (playerControlls.Player.Fire.triggered)
        {
            Debug.Log("Fire");
            inventory_scriptable_object.buildings_inventory.AddItem(items_objects[0]);
            inventory_scriptable_object.buildings_inventory.AddItem(items_objects[1]);
            EventManager.OnInventoryItemAdded();
            /*mouseHit = CastRay();
            if (mouseHit.collider != null)
            {
                Debug.Log(mouseHit.collider.name);
            }*/
            IClickable clicked_object = MouseOperations.GetClickedObject();
            if (clicked_object != null)
            {
                clicked_object.SetColor(Color.red);
                //Debug.Log(string.Format("ClickedObject get transform position, {0}", clicked_object.GetTransform().position));
                InstantiateBuilding(clicked_object);

                //InstantiateBuilding(clicked_object);
                //InstantiateBuilding();
            }

            IPhysics physics_object = MouseOperations.GetPhysicsObject();
            if (physics_object != null)
            {
                physics_object.UpdateVelocity(new Vector3(0, 50, 0));
            }


        }
        if (playerControlls.Player.Move.triggered)
        {
            Debug.Log("Move");
        }
        if (playerControlls.Player.Interact.triggered)
        {
            Debug.Log("Interact");
            button_click();
            IClickable clicked_object = MouseOperations.GetClickedObject();
            if (clicked_object != null)
            {
                //clicked_object.SetColor(Color.green);
            }
        }
    }

    /*void InstantiateBuilding(IClickable clicked_object)
    {

        BuildingScriptableObject building_object = ScriptableObject.Instantiate(selected_building_object);
        building_object.SetBuildingData(clicked_object.GetTransform().position, building_object.width, building_object.height);
        building_object.game_object = Instantiate(building_object.game_object, building_object.transform_position, Quaternion.identity);
        //building_object.game_object.GetComponent<Buildable>().SetBuildingData(building_object);

    }*/
    void preview_building()
    {
        Debug.Log(MouseOperations.GroundMousePosition(current_camera, ground_mask).point);
        preview_object = Instantiate(selected_building_object.game_object, MouseOperations.GroundMousePosition(current_camera, ground_mask).point, Quaternion.identity);
        preview_object.GetComponentInChildren<Buildable>().SetMaterial(preview_material);




    }
    void BuildableDestoryed(Buildable buildable_object)
    {
        List<(int, int)> _list = buildable_object.GetBuildingData().GetBuildingAreaList(buildable_object.GetBuildingData().position);
        foreach (var item in _list)
        {

            GridObject update_grid_object = grid_scriptable_object.grid_system_object.GetGridObject(item.Item1, item.Item2);
            update_grid_object.SetOccupied(false);
            update_grid_object.SetColor(update_grid_object.original_color);
            update_grid_object.building_data = null;
            grid_scriptable_object.grid_system_object.SetValue(item.Item1, item.Item2, update_grid_object);
        }
    }
    void InstantiateBuilding(IClickable _clicked)
    {
        Vector3 pos = Vector3.zero;
        bool can_build = true;
        if (preview_object)
        {

            pos = preview_object.transform.position;
            //IClickable clicked_object = grid_object;
        }
        else
        {
            IClickable clicked_object = MouseOperations.GetClickedObject();
            if (clicked_object != null)
            {
                pos = clicked_object.GetTransform().position;

            }
        }
        grid_scriptable_object.grid_system_object.GetXZ(_clicked.GetTransform().position, out int x, out int z);
        GridObject grid_object = grid_scriptable_object.grid_system_object.GetGridObject(x, z);
        pos = grid_object.GetTransformPosition();
        if (grid_object.GetOccupied())
        {
            Debug.Log("Cant Build Here, GetOccupied()");
        }
        else
        {
            DestroyPreview();
            BuildingScriptableObject building_ScriptableObject = ScriptableObject.Instantiate(selected_building_object);
            List<(int, int)> _list = building_ScriptableObject.GetBuildingAreaList(pos);
            if (x + building_ScriptableObject.width > grid_scriptable_object.width || z + building_ScriptableObject.height > grid_scriptable_object.depth)
            {
                Debug.Log("Cant Build Here, DestroyPreview()");
                return;
            }

            foreach (var item in _list)
            {
                GridObject update_grid_object = grid_scriptable_object.grid_system_object.GetGridObject(item.Item1, item.Item2);
                if (update_grid_object.GetOccupied())
                {
                    Debug.Log("Cant Build Here, update_grid_object");
                    can_build = false;
                    break;
                }
            }
            if (can_build)
            {
                building_ScriptableObject.SetBuildingData(pos, building_ScriptableObject.width, building_ScriptableObject.height);
                building_ScriptableObject.game_object = Instantiate(building_ScriptableObject.game_object, building_ScriptableObject.transform_position, Quaternion.identity);
                building_ScriptableObject.game_object.transform.SetParent(buildings_holder.transform);
                building_ScriptableObject.game_object.GetComponentInChildren<Buildable>().SetBuildingData(building_ScriptableObject);

                //building_ScriptableObject.game_object.GetComponent<Buildable>().SetUpPanel(canvas, current_camera);

                foreach (var item in _list)
                {
                    GridObject update_grid_object = grid_scriptable_object.grid_system_object.GetGridObject(item.Item1, item.Item2);
                    update_grid_object.SetOccupied(true);
                    update_grid_object.SetColor(Color.grey);
                    update_grid_object.building_data = building_ScriptableObject;
                    grid_scriptable_object.grid_system_object.SetValue(item.Item1, item.Item2, update_grid_object);
                }
            }
        }



    }
    void DestroyPreview()
    {
        if (preview_object)
        {
            //preview_object.GetComponent<Buildable>().DestroyPanel();
            Destroy(preview_object);
            preview_object = null;


        }

    }
    public void button_click()
    {
        //DestroyPreview();
        if (selected_building_index >= building_objects.Length)
        {
            selected_building_index = 0;
        }
        selected_building_object = building_objects[selected_building_index];
        preview_building();
        selected_building_index++;
    }

}
