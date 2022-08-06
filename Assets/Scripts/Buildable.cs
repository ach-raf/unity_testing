using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Buildable : Cuby, IClickable, IDestoryable
{
    private GameObject _instantiated_panel;

    private MeshRenderer mesh_renderer;
    private Vector3 position;
    BuildingScriptableObject building_data;
    private void Awake()
    {
        mesh_renderer = GetComponent<MeshRenderer>();
    }

    public void SetMaterial(Material _material)
    {
        mesh_renderer.material = _material;
    }

    public void DestroyObject()
    {
        gameObject.transform.parent.gameObject.SetActive(false);
    }

    public void SetBuildingData(BuildingScriptableObject _building_data)
    {
        building_data = _building_data;
    }
    public BuildingScriptableObject GetBuildingData()
    {
        return building_data;
    }

    public new void click()
    {
        EventManager.OnBuildableClicked(this);
    }
    public new void right_click()
    {
        DestroyBuilding();
        EventManager.OnBuildableRightClicked(this);
    }

    public void SetUpPanel(GameObject current_canvas, Camera current_camera)
    {
        Vector3 pos = new Vector3(building_data.position.x + 2.5f, building_data.position.y + 10, building_data.position.z + 2.5f);
        _instantiated_panel.transform.position = pos;
        _instantiated_panel.transform.SetParent(current_canvas.transform);
    }

    public void DestroyPanel()
    {
        Destroy(_instantiated_panel);

    }
    public void DestroyBuilding()
    {
        for (int i = 0; i < transform.parent.transform.childCount; i++)
        {
            Destroy(transform.parent.transform.GetChild(i).gameObject);
        }
        building_data.DestroyBuilding();
    }





}
