using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuUIControl : MonoBehaviour
{
    public event Action<string> SendInfo = delegate { };

    [Header("UI Cursor Components")]
    [SerializeField] Image rCursor = null, zCursor = null;
    [SerializeField] Transform rTransform = null, zTransform = null;
    [SerializeField] Cursor cursor = null;
    [SerializeField] AudioSource moveAudio = null;

    [Header("UI Cursor Settings")]
    [SerializeField] float sizeModifier = 1.2f;

    // necessary scripts
    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;
    
    // bools to track cursor position;
    bool onR = false, onZ = false;
    
    // bool that coordinates different actions between triggers and directional inputs
    bool changeBool = true;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        menuRotation = FindObjectOfType<MenuRotation>();
    }


    #region event subscriptions
    private void OnEnable()
    {
        playerInput.HorizontalCursorMovement += IconControl;
        menuRotation.AnimationStarted += HideIcons;
        menuRotation.AnimationFinished += ActivateOppositeIcon;
        menuRotation.CloseAnimationStarted += CloseIcons;
    }

    private void OnDisable()
    {
        playerInput.HorizontalCursorMovement -= IconControl;
        menuRotation.AnimationStarted -= HideIcons;
        menuRotation.AnimationFinished -= ActivateOppositeIcon;
        menuRotation.CloseAnimationStarted -= CloseIcons;
    }
    #endregion

    
    // sets both cursors inactive on start
    private void Start()
    {
        rCursor.enabled = false;
        zCursor.enabled = false;
    }


    // manages player inputs on the icons when active
    void IconControl(float xInput)
    {
        if(onZ || onR)
        {
            // if the cursor is moving in the direction of either icon, rotates menu, sets changeBool to false so it doesn't automatically select right
            if(xInput < 0 && onZ)
            {
                changeBool = false;
                menuRotation.StartRotation(Vector3.up);
            }
            else if (xInput > 0 && onR)
            {
                changeBool = false;
                menuRotation.StartRotation(-Vector3.up);
            }
                

            // if the cursor moves inward on the item screen, activates item cursor
            else if (menuRotation.CurrentMenu == "item")
            {
                SendInfo?.Invoke("");
                cursor.ShowCursor(xInput);
                moveAudio.Play();
                HideIcons();
                CloseIcons();
            }

            // if the cursor moves inward on another screen, activates opposite cursor, sets changeBool to shift to right
            else if (menuRotation.CurrentMenu != "item")
            {
                changeBool = false;
                ActivateOppositeIcon();
                moveAudio.Play();
            }
        }
    }


    // activates R icon by manipulating bools -- called outside
    public void ActivateRIcon()
    {
        onZ = true;
        changeBool = false;
        ActivateOppositeIcon();
    }


    // activates Z icon by manipulating bools -- called outside
    public void ActivateZIcon()
    {
        onR = true;
        changeBool = false;
        ActivateOppositeIcon();
    }


    // cordinates the icons to switch following the menu rotation
    void ActivateOppositeIcon()
    {
        // if changeBool has not been set, automatically sets both to false to activate next loop (setting to right when rotated with triggers)
        if(changeBool)
        {
            onR = false;
            onZ = false;
        }

        // if both cursors are inactive or Z is active, then  sets the right cursor
        if ((onR == false && onZ == false) || (onR == false && onZ == true))
        {
            rCursor.enabled = true;
            onR = true;
            rTransform.localScale = new Vector3(sizeModifier, sizeModifier, 1);

            zCursor.enabled = false;
            onZ = false;
            zTransform.localScale = new Vector3(1, 1, 1);

            DetermineRAction();
        }

        // if R is active, sets Z cursor
        else if (onR == true && onZ == false)
        {
            zCursor.enabled = true;
            onZ = true;
            zTransform.localScale = new Vector3(sizeModifier, sizeModifier, 1);

            rCursor.enabled = false;
            onR = false;
            rTransform.localScale = new Vector3(1, 1, 1);

            DetermineZAction();
        }

        changeBool = true;
    }


    // sets both icons to false on close
    void CloseIcons()
    {
        onR = false;
        onZ = false;     

        SendInfo?.Invoke("");
    }


    // hides icons once the player activates the cursor
    void HideIcons()
    {
        // if R is enabled, disables
        if (rCursor.enabled == true)
        {
            rTransform.localScale = new Vector3(1, 1, 1);
            rCursor.enabled = false;
        }
            
        
        // if Z is enabled, disables
        if (zCursor.enabled == true)
        {
            zTransform.localScale = new Vector3(1, 1, 1);
            zCursor.enabled = false;
        }
            
    }


    // sends the appropriate action command for each possible R placement
    void DetermineRAction()
    {
        string infoToSend = "";
        if (menuRotation.CurrentMenu == "item")
            infoToSend = "To Map";
        else if (menuRotation.CurrentMenu == "map")
            infoToSend = "To Quest Status";
        else if (menuRotation.CurrentMenu == "quest")
            infoToSend = "To Equipment";
        else if (menuRotation.CurrentMenu == "equip")
            infoToSend = "To Select Item";

        SendInfo?.Invoke(infoToSend);
    }


    // sends the appropriate action command for each possible Z placement
    void DetermineZAction()
    {
        string infoToSend = "";
        if (menuRotation.CurrentMenu == "item")
            infoToSend = "To Equipment";
        else if (menuRotation.CurrentMenu == "equip")
            infoToSend = "To Quest Status";
        else if (menuRotation.CurrentMenu == "quest")
            infoToSend = "To Map";
        else if (menuRotation.CurrentMenu == "map")
            infoToSend = "To Select Item";
        
        SendInfo?.Invoke(infoToSend);
    }
}
