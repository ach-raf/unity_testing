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
    public ItemScriptableObject selected_building_object;
    public Material preview_material;
    private GameObject preview_object = null;
    private IClickable clicked_object;


    private void Awake()
    {
        playerControlls = new PlayerControlls();
    }

    private void OnEnable()
    {
        playerControlls.Enable();
        EventManager.InventoryItemClicked += InventoryItemClicked;
        EventManager.CubyClicked += CubyClicked;
        EventManager.BuildingRotatePerformed += BuildingRotatePerformed;


    }
    private void OnDisable()
    {
        playerControlls.Disable();
        EventManager.InventoryItemClicked -= InventoryItemClicked;
        EventManager.CubyClicked -= CubyClicked;
        EventManager.BuildingRotatePerformed -= BuildingRotatePerformed;


    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        ShowPreviewAtMousePosition();
        if (playerControlls.Player.Fire.triggered)
        {
            Debug.Log("Fire");
            clicked_object = MouseOperations.ClickedObject();
            if (clicked_object != null)
            {
                clicked_object.click();
                return;
            }
            else
            {
                Debug.Log("Clicked object null buildingManager");
            }

        }
        if (playerControlls.Player.Interact.triggered)
        {
            Debug.Log("Interact");
            clicked_object = MouseOperations.ClickedObject();

            if (clicked_object != null)
            {
                clicked_object.right_click();
                return;
            }
        }
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

    public void InventoryItemClicked(ItemScriptableObject item_scriptable_object)
    {
        selected_building_object = item_scriptable_object;
        CreateBuildingPreview();
    }

    public void CubyClicked(Cuby cuby_component)
    {
        IClickable cuby_clickable = cuby_component.GetComponent<IClickable>();
        if (cuby_component.GetGridObject().GetOccupied())
        {
            return;
        }
        if (!preview_object)
        {
            return;
        }

        Vector3 position = cuby_component.GetTransform().position;
        cuby_component.GetXZ(out int x, out int z);
        Debug.Log("x: " + x + " z: " + z);
        Quaternion preview_object_rotation = preview_object.transform.rotation;
        DestroyPreview();
        BuildingScriptableObject building_ScriptableObject = (BuildingScriptableObject)ScriptableObject.Instantiate(selected_building_object);
        List<(int, int)> _list = building_ScriptableObject.GetBuildingAreaList(x, z);
        for (int i = x; i < x + building_ScriptableObject.width; i += 1)
        {
            for (int j = z; j < z + building_ScriptableObject.height; j += 1)
            {
                Debug.Log((i, j));
            }
        }
        if (x + building_ScriptableObject.width > grid_scriptable_object.x || z + building_ScriptableObject.height > grid_scriptable_object.z)
        {
            Debug.Log("Cant Build Here, DestroyPreview()");
            return;
        }
        bool can_build = true;
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
            building_ScriptableObject.SetBuildingData(grid_scriptable_object.grid_system_object, position, building_ScriptableObject.width, building_ScriptableObject.height);
            building_ScriptableObject.gameObject = Instantiate(building_ScriptableObject.gameObject, building_ScriptableObject.transform_position, preview_object_rotation);
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
    public void ShowPreviewAtMousePosition()
    {
        if (preview_object)
        {
            Vector3 mouse_position = MouseOperations.GroundMousePosition(ground_mask).point;
            int x = (int)mouse_position.x;
            int z = (int)mouse_position.z;
            int y = (int)mouse_position.y;
            Vector3 snap_position = new Vector3(x, 2.5f, z);
            preview_object.transform.position = snap_position;

        }
    }
    void CreateBuildingPreview(Vector3 preview_postion = default(Vector3))
    {
        if (preview_object)
        {
            Destroy(preview_object);
        }
        if (preview_postion == default(Vector3))
        {
            preview_postion = MouseOperations.GroundMousePosition(ground_mask).point;
            int x = (int)preview_postion.x;
            int z = (int)preview_postion.z;
            int y = (int)preview_postion.y;
            preview_postion = new Vector3(x, 2.5f, z);

        }
        preview_object = Instantiate(selected_building_object.gameObject, preview_postion, Quaternion.identity);
        preview_object.GetComponentInChildren<Renderer>().material = preview_material;
    }

    public void BuildingRotatePerformed()
    {
        if (preview_object)
        {
            preview_object.transform.Rotate(0, 90, 0);
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


}
