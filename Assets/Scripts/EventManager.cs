using System;
using UnityEngine.Events;

public static class EventManager
{

    public static event Action<ItemScriptableObject> InventoryItemAdded;
    public static void OnInventoryItemAdded(ItemScriptableObject item_scriptable_object) => InventoryItemAdded?.Invoke(item_scriptable_object);

    public static event Action<ItemScriptableObject> InventoryItemClicked;
    public static void OnInventoryItemClicked(ItemScriptableObject item_scriptable_object) => InventoryItemClicked?.Invoke(item_scriptable_object);

    public static event Action<Buildable> BuildableClicked;
    public static void OnBuildableClicked(Buildable buildable_component) => BuildableClicked?.Invoke(buildable_component);


    public static event Action<Buildable> BuildableRightClicked;
    public static void OnBuildableRightClicked(Buildable buildable_component) => BuildableRightClicked?.Invoke(buildable_component);

    public static event Action<Cuby> CubyClicked;
    public static void OnCubyClicked(Cuby cuby_component) => CubyClicked?.Invoke(cuby_component);

    public static event Action DebugResourceAdded;
    public static void OnDebugResourceAdded() => DebugResourceAdded?.Invoke();

    public static event Action RefreshBuildingPanel;
    public static void OnRefreshBuildingPanel() => RefreshBuildingPanel?.Invoke();

    public static event Action BuildingRotatePerformed;
    public static void OnBuildingRotatePerformed() => BuildingRotatePerformed?.Invoke();



}
