using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Item Item { get; private set; }
    public int State { get; private set; } = 1;

    // images for visuals
    [SerializeField] Image icon = null;
    [SerializeField] Image selectedItem = null;

    // item components
    Text ammo = null;
    string itemName = null;


    // caches for necessary components
    private void Awake()
    {
        ammo = GetComponentInChildren<Text>();
    }


    // initializes item icon references
    public void Init(Item itemReference)
    {
        Item = itemReference;
    }


    // sets icon components equal to components in this script
    private void Start()
    {
        // if item exists, sets state and renders item components if necessary
        if(Item != null)
        {
            State = Item.itemState;
            if (State > 0)
            {
                icon.sprite = Item.itemIcon;
                itemName = Item.itemName;
                if (Item.maxAmmo != -1)
                    ammo.text = Item.maxAmmo.ToString();
            }
            else if (State == 0)
            {
                // renders objects as invisible if state is set to unobtained
                Color color = Color.white;
                color.a = 0f;
                icon.color = color;
            }
        }
    }


    public void EquipItem()
    {

    }
}
