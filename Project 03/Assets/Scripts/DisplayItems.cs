using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayItems : MonoBehaviour
{
    [SerializeField] List<Item> items = new List<Item>();
    [SerializeField] GameObject itemCellPrefab = null;
    List<GameObject> itemCells = new List<GameObject>();

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

        itemCells.Add(newItem);

        MenuItem itemScript = newItem.GetComponent<MenuItem>();
        itemScript?.Init(currentItem);
    }


    // sets the graphic for equipped when called from c buttons - NOTE, this is NOT MODULAR, and works specifically with the child arrangement of the prefab
    public void SetEquippedGraphics(Image cLeft, Image cDown, Image cRight)
    {
        foreach(GameObject itemCell in itemCells)
        {
            // finds equip graphic
            Image itemGraphic = itemCell.gameObject.transform.GetChild(1).GetComponent<Image>();
            Image equipGraphic = itemCell.gameObject.transform.GetChild(0).GetComponent<Image>();
            

            // if the item graphic equals any of the c buttons, sets the graphic to enabled, else sets it to false
            if (itemGraphic.sprite == cLeft.sprite || itemGraphic.sprite == cDown.sprite || itemGraphic.sprite == cRight.sprite)
                equipGraphic.enabled = true;
            else if (itemGraphic.sprite != cLeft.sprite && itemGraphic.sprite != cDown.sprite && itemGraphic.sprite != cRight.sprite)
                equipGraphic.enabled = false;
        }
    }
}
