using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Item : MonoBehaviour
{
    private ItemScriptableObject item_scriptable_object;
    public ItemScriptableObject ItemScriptableObject
    {
        get { return item_scriptable_object; }
        set { item_scriptable_object = value; }
    }
    public void SetItem()
    {
        var ItemName = gameObject.transform.Find("ItemName");
        var ItemImage = gameObject.transform.Find("ItemImage");
        //var ItemQuantity = gameObject.transform.Find("ItemQuantity");
        ItemName.GetComponent<TextMeshProUGUI>().text = item_scriptable_object.name;
        ItemImage.GetComponent<Image>().sprite = item_scriptable_object.ItemImage;
        //ItemQuantity.GetComponent<TextMeshProUGUI>().text = item_scriptable_object.quantity.ToString();


    }
    public void ItemClicked()
    {
        // set at the inspector on the item panel
        Debug.Log(item_scriptable_object.name);
        //todo
        //buildable.GetTransform().parent.gameObject.SetActive(true);

        EventManager.OnInventoryItemClicked(item_scriptable_object);
    }
}
