using System;
using UnityEngine.Events;

public static class EventManager
{

    public static event Action InventoryItemAdded;
    public static void OnInventoryItemAdded() => InventoryItemAdded?.Invoke();

    public static event Action<BuildingScriptableObject> InventoryItemClicked;
    public static void OnInventoryItemClicked(BuildingScriptableObject item_scriptable_object) => InventoryItemClicked?.Invoke(item_scriptable_object);


}
