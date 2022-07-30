using System;
using UnityEngine.Events;

public static class EventManager
{

    public static event Action InventoryItemAdded;
    public static void OnInventoryItemAdded() => InventoryItemAdded?.Invoke();


}
