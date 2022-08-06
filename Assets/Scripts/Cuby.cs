using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuby : MonoBehaviour, IClickable
{
    private GridObject grid_object;

    public GridObject GetGridObject()
    {
        return grid_object;
    }
    public void SetGridObject(GridObject _grid_object)
    {
        grid_object = _grid_object;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public void SetColor(Color _color)
    {
        GetComponent<Renderer>().material.color = _color;
    }
    public void click()
    {
        EventManager.OnCubyClicked(this);
    }
    public void right_click()
    {
    }
    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public void GetXZ(out int x, out int z)
    {
        x = grid_object.grid_x_position;
        z = grid_object.grid_z_position;
    }
}
