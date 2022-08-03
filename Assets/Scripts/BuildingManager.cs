using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    //public BuildingScriptableObject[] building_objects;

    [SerializeField] LayerMask ground_mask;
    [SerializeField] LayerMask buildings_mask;

    [SerializeField] GameObject buildings_holder;

    public PlayerControlls playerControlls;
    public GridSystemScriptableObject grid_scriptable_object;
    public BuildingScriptableObject selected_building_object;
    public Material preview_material;
    private GameObject preview_object = null;
    private IClickable clicked_building_object = null;
    private IClickable clicked_ground_object = null;


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


    // Update is called once per frame
    void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        if (preview_object)
        {
            preview_object.transform.position = MouseOperations.GroundMousePosition(ground_mask).point;

        }
        if (playerControlls.Player.Fire.triggered)
        {
            Debug.Log("Fire");
            clicked_building_object = MouseOperations.GetClickedObject(buildings_mask);
            if (clicked_building_object != null)
            {
                if (clicked_building_object.GetGameObject().GetComponent<Buildable>())
                {
                    clicked_building_object.GetGameObject().GetComponent<Buildable>().click();

                }
                return;
            }

            clicked_ground_object = MouseOperations.GetClickedObject(ground_mask);
            if (clicked_ground_object == null)
            {
                Debug.Log("clicked_ground_object is null");
            }

            if (clicked_ground_object != null)
            {
                clicked_ground_object.SetColor(Color.red);
                InstantiateBuilding(clicked_ground_object);
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
            clicked_building_object = MouseOperations.GetClickedObject(buildings_mask);
            if (clicked_building_object != null)
            {
                if (clicked_building_object.GetGameObject().GetComponent<Buildable>())
                {
                    clicked_building_object.GetGameObject().GetComponent<Buildable>().DestroyBuilding(grid_scriptable_object);

                }
                return;
                //clicked_object.SetColor(Color.green);
            }
        }
    }

    void preview_building(Vector3 preview_postion = default(Vector3))
    {
        if (preview_object)
        {
            Destroy(preview_object);
        }
        if (preview_postion == default(Vector3))
        {
            preview_postion = MouseOperations.GroundMousePosition(ground_mask).point;
            preview_postion.y = 5;
        }
        preview_object = Instantiate(selected_building_object.gameObject, preview_postion, Quaternion.identity);
        preview_object.GetComponentInChildren<Renderer>().material = preview_material;
    }
    void BuildableDestoryed(Buildable buildable_object)
    {
        List<(int, int)> _list = buildable_object.GetBuildingData().GetBuildingAreaList(buildable_object.GetBuildingData().position);
        foreach (var item in _list)
        {

            GridObject update_grid_object = grid_scriptable_object.grid_system_object.GetGridObject(item.Item1, item.Item2);
            update_grid_object.SetOccupied(false);
            //update_grid_object.SetColor(update_grid_object.original_color);
            update_grid_object.SetMaterial(update_grid_object.original_material);
            update_grid_object.SetBuildingData(null);
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
            IClickable clicked_object = MouseOperations.GetClickedObject(ground_mask);
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
            if (x + building_ScriptableObject.width > grid_scriptable_object.x || z + building_ScriptableObject.height > grid_scriptable_object.z)
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
                building_ScriptableObject.SetBuildingData(pos, building_ScriptableObject.width, building_ScriptableObject.height, new Inventory());
                building_ScriptableObject.gameObject = Instantiate(building_ScriptableObject.gameObject, building_ScriptableObject.transform_position, Quaternion.identity);
                building_ScriptableObject.gameObject.transform.SetParent(buildings_holder.transform);
                building_ScriptableObject.gameObject.GetComponentInChildren<Buildable>().SetBuildingData(building_ScriptableObject);

                //building_ScriptableObject.prefab.GetComponent<Buildable>().SetUpPanel(canvas, current_camera);

                foreach (var item in _list)
                {
                    GridObject update_grid_object = grid_scriptable_object.grid_system_object.GetGridObject(item.Item1, item.Item2);
                    update_grid_object.SetOccupied(true);
                    update_grid_object.SetColor(Color.grey);
                    update_grid_object.SetBuildingData(building_ScriptableObject);
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
    public void InventoryItemClicked(BuildingScriptableObject item_scriptable_object)
    {
        selected_building_object = item_scriptable_object;
        preview_building();
    }


}
