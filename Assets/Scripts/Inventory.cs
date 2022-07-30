using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<ItemScriptableObject> items;

    public Inventory()
    {
        items = new List<ItemScriptableObject>();
    }
    public List<ItemScriptableObject> GetItems()
    {
        return items;
    }
    public void AddItem(ItemScriptableObject item)
    {
        items.Add(item);
    }
    public void RemoveItem(ItemScriptableObject item)
    {
        items.Remove(item);
    }
    public void RemoveItem(int index)
    {
        items.RemoveAt(index);
    }
    public void RemoveAllItems()
    {
        items.Clear();
    }
    public int GetItemCount()
    {
        return items.Count;
    }
    public ItemScriptableObject GetItem(int index)
    {
        return items[index];
    }
    public ItemScriptableObject GetItem(string name)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                return item;
            }
        }
        return null;
    }
    public ItemScriptableObject GetItem(ItemScriptableObject item)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                return item_;
            }
        }
        return null;
    }
    public bool ContainsItem(ItemScriptableObject item)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                return true;
            }
        }
        return false;
    }
    public bool ContainsItem(string name)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                return true;
            }
        }
        return false;
    }
    public bool ContainsItem(int id)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                return true;
            }
        }
        return false;
    }
    public int GetItemIndex(ItemScriptableObject item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == item.name)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetItemIndex(string name)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].name == name)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetItemIndex(int id)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].id == id)
            {
                return i;
            }
        }
        return -1;
    }
    public int GetItemQuantity(ItemScriptableObject item)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                return item_.quantity;
            }
        }
        return 0;
    }
    public int GetItemQuantity(string name)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                return item.quantity;
            }
        }
        return 0;
    }
    public int GetItemQuantity(int id)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                return item.quantity;
            }
        }
        return 0;
    }
    public void SetItemQuantity(ItemScriptableObject item, int quantity)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                item_.quantity = quantity;
            }
        }
    }
    public void SetItemQuantity(string name, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                item.quantity = quantity;
            }
        }
    }
    public void SetItemQuantity(int id, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                item.quantity = quantity;
            }
        }
    }
    public void AddItemQuantity(ItemScriptableObject item, int quantity)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                item_.quantity += quantity;
            }
        }
    }
    public void AddItemQuantity(string name, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                item.quantity += quantity;
            }
        }
    }
    public void AddItemQuantity(int id, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                item.quantity += quantity;
            }
        }
    }
    public void RemoveItemQuantity(ItemScriptableObject item, int quantity)
    {
        foreach (ItemScriptableObject item_ in items)
        {
            if (item_.name == item.name)
            {
                item_.quantity -= quantity;
            }
        }
    }
    public void RemoveItemQuantity(string name, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                item.quantity -= quantity;
            }
        }
    }
    public void RemoveItemQuantity(int id, int quantity)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                item.quantity -= quantity;
            }
        }
    }
    public void RemoveAllItemQuantities()
    {
        foreach (ItemScriptableObject item in items)
        {
            item.quantity = 0;
        }
    }
    public void RemoveAllItemQuantities(string name)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.name == name)
            {
                item.quantity = 0;
            }
        }
    }
    public void RemoveAllItemQuantities(int id)
    {
        foreach (ItemScriptableObject item in items)
        {
            if (item.id == id)
            {
                item.quantity = 0;
            }
        }
    }


}

