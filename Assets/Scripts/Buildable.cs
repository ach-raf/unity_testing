using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Buildable : Cuby, IClickable, IDestoryable
{
    private GameObject _instantiated_panel;
    private bool _panel_state = true;

    private MeshRenderer mesh_renderer;
    private Vector3 position;
    BuildingScriptableObject building_data;
    private void Awake()
    {
        mesh_renderer = GetComponent<MeshRenderer>();
        //_instantiated_panel = Instantiate(panel);

    }


    public void SetMaterial(Material _material)
    {
        mesh_renderer.material = _material;
    }

    public void DestroyObject()
    {
        //EventManager.OnBuildableDestroy(this);
        //DestroyPanel();
        Destroy(gameObject);
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
        Debug.Log($"{building_data.name} clicked");
        Debug.Log("Click from buildable");
        EventManager.OnBuildableClicked(this);
    }
    public new void right_click()
    {
        Debug.Log($"{building_data.name} right clicked");
        _instantiated_panel.SetActive(_panel_state);
        _panel_state = !_panel_state;
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




}
