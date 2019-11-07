using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] GameObject itemCellPrefab = null;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Item item in items)
        {
            CreateMenuItem(item);
        }
    }

    // instantiates a new menu item
    void CreateMenuItem(Item currentItem)
    {
        GameObject newItem = Instantiate
            (itemCellPrefab, transform);

        MenuItem itemScript = newItem.GetComponent<MenuItem>();
        itemScript?.Init(currentItem);
    }
}
