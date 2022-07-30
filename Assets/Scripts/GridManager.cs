using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{

    public GridScriptableObject grid_scriptable_object;
    public GridObject tile_object;

    void Awake()
    {
        grid_scriptable_object.grid_system_object = new GridSys<GridObject>(grid_scriptable_object.width, grid_scriptable_object.depth, grid_scriptable_object.cell_size, grid_scriptable_object.origin_position,
                                                    (GridSys<GridObject> grid, int x, int y, int z) => CreateGridObject(new Vector3(x, y, z), grid));

    }
    // Start is called before the first frame update
    void Start()
    {
        InstantiateGridObjects();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private GridObject CreateGridObject(Vector3 _position, GridSys<GridObject> _grid)
    {
        GridObject tile = ScriptableObject.Instantiate(tile_object);
        return tile.Init(_position, grid_scriptable_object.grid_system_object);
    }
    void InstantiateGridObjects()
    {
        GridObject grid_object;
        for (int x = 0; x < grid_scriptable_object.width; x++)
        {
            for (int z = 0; z < grid_scriptable_object.depth; z++)
            {
                GameObject spawn_object = Instantiate(tile_object.game_object);
                spawn_object.transform.position = grid_scriptable_object.grid_system_object.GetWorldPosition(x, z);
                spawn_object.transform.SetParent(transform);
                spawn_object.name = $"{x}, {z}";

                grid_object = grid_scriptable_object.grid_system_object.GetGridObject(x, z);
                grid_object.SetGameObject(spawn_object);
                grid_object.SetGrid(grid_scriptable_object.grid_system_object);
                bool is_offset = (x + z) % 2 == 0;
                if (is_offset)
                {
                    grid_object.SetColor(Color.green);
                }
                grid_object.original_color = grid_object.GetColor();
                grid_object.SetOccupied(false);
                grid_scriptable_object.grid_system_object.SetValue(x, z, grid_object);

            }
        }
    }
}
