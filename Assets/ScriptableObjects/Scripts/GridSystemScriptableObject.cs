using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "GridSystem ScriptableObject", menuName = "Grid/GridSystem ScriptableObject", order = 0)]
public class GridSystemScriptableObject : ScriptableObject
{
    public new string name;
    public int x; //width
    public int y = 0;
    public int z; //height or depth

    public float cell_size = 5;

    public Vector3 origin_position = new Vector3(0, 0, 0);
    public GridSys<GridObject> grid_system_object;
    //public BuildingScriptableObject[,] buildings_list;

    private void OnEnable()
    {

    }


}

