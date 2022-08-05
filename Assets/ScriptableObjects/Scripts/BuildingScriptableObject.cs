using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName = "Building ScriptableObject", menuName = "Building/Building ScriptableObject", order = 1)]
public class BuildingScriptableObject : ItemScriptableObject
{

    public GridSys<GridObject> grid_system_object;
    public List<ResourcesScriptableObject> resources_generated;
    public float cell_size = 5;
    public int width;
    public int height;
    public Vector3 offset;
    public static int id_counter = 0;
    public int id_building;

    public Vector3 position;
    public Vector3 transform_position;
    public RessourceType ressource_type = RessourceType.None;

    private float click_error_tolerance = 1.5f;
    public enum RessourceType
    {
        None,
        Standard,
        Premium
    }

    public void SetBuildingData(GridSys<GridObject> _grid_system_object, Vector3 _position, int _width, int _height, RessourceType _ressource_type = RessourceType.None)
    {

        this.grid_system_object = _grid_system_object;
        this.id_building = id_counter;
        this.width = _width;
        this.height = _height;
        this.inventory = new Inventory();
        this.transform_position = CalculateOffsetPosition(_position);
        this.position = new Vector3(_position.x, _position.y + 1, _position.z);
        this.name = $"({position.x}, {position.y}, {position.z})";
        this.ressource_type = _ressource_type;
        id_counter++;
    }
    public override string ToString()
    {
        return $"Building: {name}";
    }

    public Vector3 CalculateOffsetPosition(Vector3 _position)
    {
        float x = _position.x + offset.x;
        float y = _position.y + offset.y;
        float z = _position.z - offset.z;
        return new Vector3(x, y, z);
    }
    public void GetXZ(Vector3 world_position, out int x, out int z)
    {

        int temp_x = Mathf.FloorToInt((world_position).x / cell_size);
        int temp_z = Mathf.FloorToInt((world_position).z / cell_size);

        if (world_position.x - click_error_tolerance <= temp_x || world_position.x - click_error_tolerance >= temp_x && world_position.z - click_error_tolerance <= temp_z || world_position.z - click_error_tolerance >= temp_z)
        {
            x = temp_x;
            z = temp_z;
        }
        else
        {
            x = -1;
            z = -1;

        }
    }

    public List<(int, int)> GetBuildingAreaList(Vector3 starting_position)
    {
        GetXZ(starting_position, out int x, out int z);
        List<(int, int)> result = new List<(int, int)>();

        for (int i = x; i < x + width; i += 1)
        {
            for (int j = z; j < z + height; j += 1)
            {
                result.Add((i, j));
            }
        }
        return result;
    }
    public List<(int, int)> GetBuildingAreaList(int x, int z)
    {

        List<(int, int)> result = new List<(int, int)>();

        for (int i = x; i < x + width; i += 1)
        {
            for (int j = z; j < z + height; j += 1)
            {
                result.Add((i, j));
            }
        }
        return result;
    }

    public Inventory GetInventory()
    {
        return inventory;
    }
    public void SetInventory(Inventory _inventory)
    {
        inventory = _inventory;
    }
    public List<(int, int)> Buildable_list(Vector3 starting_position, GridSystemScriptableObject grid_object)
    {

        List<(int, int)> result = new List<(int, int)>();
        List<(int, int)> _list = GetBuildingAreaList(starting_position);
        for (int i = 0; i < _list.Count; i += 1)
        {
            GridObject spawn_tile = grid_object.grid_system_object.GetGridObject(_list[i].Item1, _list[i].Item2);
            if (spawn_tile.GetOccupied())
            {
                return null;
            }
            else
            {
                result.Add((_list[i].Item1, _list[i].Item2));
            }
        }

        return result;
    }

    public void DestroyBuilding()
    {

        List<(int, int)> _list = GetBuildingAreaList(position);
        for (int i = 0; i < _list.Count; i += 1)
        {
            GridObject tile = grid_system_object.GetGridObject(_list[i].Item1, _list[i].Item2);
            tile.EmptyTileObject();
            grid_system_object.SetValue(tile.GetPosition(), tile);
        }

    }
    public IEnumerator StartProcessingResources()
    {
        Debug.Log("Start processing resources");
        yield return new WaitForSeconds(1);
        foreach (var resource in resources_generated)
        {
            inventory.AddItem(resource);
        }
        Debug.Log("Finished processing resources");

    }





}
