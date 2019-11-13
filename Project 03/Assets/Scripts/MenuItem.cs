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

    /*
    #region subscriptions
    private void OnEnable()
    {
        playerInput.CPress += SetUnequipped;
    }

    private void OnDisable()
    {
        playerInput.CPress -= SetUnequipped;
    }
    #endregion
    */

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
            // determines
            GameObject cButtonReference = null;
            if (cButton == "left")
                cButtonReference = GameObject.Find("MainUI_cnv/Actions_pnl/Cleft_img");
            if (cButton == "down")
                cButtonReference = GameObject.Find("MainUI_cnv/Actions_pnl/Cdown_img");
            if (cButton == "right")
                cButtonReference = GameObject.Find("MainUI_cnv/Actions_pnl/Cright_img");

            CButtonBehavior cButtonBehavior = cButtonReference.GetComponent<CButtonBehavior>();
            if (cButtonBehavior != null)
                cButtonBehavior.SpawnPrefab(transform.position, icon);

            // calls to set equipped
            SetEquipped(cButton);
        }
    }


    // sets the equipped graphic
    void SetEquipped(string cPress)
    {
        equipped = true;
        selectedItem.enabled = true;
        equipLocation = cPress;
    }
    
}
