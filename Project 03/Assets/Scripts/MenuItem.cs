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
    public string itemName = null;

    PlayerInput playerInput = null;
    string equipLocation = "none";

    // secondary bool is used to prevent items from unequipping on the same frame
    bool equipped = false;
    bool equipGuard = false;


    // caches for necessary components
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
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
        if (Item != null)
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


    // sets c button reference and calls button to activate animation
    public void ItemAnimation(string cButton)
    {
        if (equipped == false)
        {
            CButtonBehavior cButtonBehavior = FindObjectOfType<CButtonBehavior>();
            if (cButtonBehavior != null)
                cButtonBehavior.SpawnPrefab(transform.position, icon, cButton, Item.maxAmmo);
        }
    }   
}
