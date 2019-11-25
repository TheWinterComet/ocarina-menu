using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MenuUIControl : MonoBehaviour
{
    public event Action<string> SendInfo = delegate { };

    [SerializeField] Image RCursor = null, ZCursor = null;
    [SerializeField] Cursor cursor = null;
    [SerializeField] AudioSource moveAudio = null;

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
        RCursor.enabled = false;
        ZCursor.enabled = false;
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
            RCursor.enabled = true;
            onR = true;

            ZCursor.enabled = false;
            onZ = false;

            DetermineRAction();
        }

        // if R is active, sets Z cursor
        else if (onR == true && onZ == false)
        {
            ZCursor.enabled = true;
            onZ = true;

            RCursor.enabled = false;
            onR = false;

            DetermineZAction();
        }

        changeBool = true;
    }


    // sets both icons to false on close
    void CloseIcons()
    {
        onR = false;
        onZ = false;
    }


    // hides icons once the player activates the cursor
    void HideIcons()
    {
        // if R is enabled, disables
        if (RCursor.enabled == true)
            RCursor.enabled = false;
        
        // if Z is enabled, disables
        if (ZCursor.enabled == true)
            ZCursor.enabled = false;
    }

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
