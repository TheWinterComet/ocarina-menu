using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuItem : MonoBehaviour
{
    public Item item { get; private set; }
    public int state { get; private set; } = 1;

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
        item = itemReference;
    }


    // sets icon components equal to components in this script
    private void Start()
    {
        // if item exists, sets state and renders item components if necessary
        if(item != null)
        {
            state = item.itemState;
            if (state > 0)
            {
                icon.sprite = item.itemIcon;
                itemName = item.itemName;
                if (item.maxAmmo != -1)
                    ammo.text = item.maxAmmo.ToString();
            }
            else if (state == 0)
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
