using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    [SerializeField] Image RCursor = null, ZCursor = null;
    [SerializeField] Cursor cursor = null;
    [SerializeField] AudioSource moveAudio = null;

    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;
    bool onR = false, onZ = false;

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
    }

    private void OnDisable()
    {
        playerInput.HorizontalCursorMovement -= IconControl;
        menuRotation.AnimationStarted -= HideIcons;
        menuRotation.AnimationFinished -= ActivateOppositeIcon;
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
        Debug.Log(menuRotation.CurrentMenu);
        if (onZ == true && onR == false)
        {
            if (xInput > 0 && menuRotation.CurrentMenu == "item")
            {
                cursor.ShowCursor();
                moveAudio.Play();
                HideIcons();
            }
            else if (xInput > 0 && menuRotation.CurrentMenu != "item")
            {
                ActivateOppositeIcon();
                moveAudio.Play();
            }  
            else if (xInput < 0)
                menuRotation.StartRotation(Vector3.up);
        }

        else if(onR == true && onZ == false)
        {
            if (xInput < 0 && menuRotation.CurrentMenu == "item")
            {
                cursor.ShowCursor();
                moveAudio.Play();
                HideIcons();
            }
            else if (xInput < 0 && menuRotation.CurrentMenu != "item")
            {
                ActivateOppositeIcon();
                moveAudio.Play();
            }
            else if (xInput > 0)
                menuRotation.StartRotation(-Vector3.up);   
        }
    }


    // activates R icon by manipulating bools -- called outside
    public void ActivateRIcon()
    {
        onZ = true;
        ActivateOppositeIcon();
    }


    // activates Z icon by manipulating bools -- called outside
    public void ActivateZIcon()
    {
        onR = true;
        ActivateOppositeIcon();
    }


    // cordinates the icons to switch following the menu rotation
    void ActivateOppositeIcon()
    {
        // if both cursors are inactive or Z is active, then  sets the right cursor
        if ((onR == false && onZ == false) || (onR == false && onZ == true))
        {
            RCursor.enabled = true;
            onR = true;

            ZCursor.enabled = false;
            onZ = false;
        }

        // if R is active, sets Z cursor
        else if (onR == true && onZ == false)
        {
            ZCursor.enabled = true;
            onZ = true;

            RCursor.enabled = false;
            onR = false;
        }

        Debug.Log("onZ: " + onZ);
        Debug.Log("onR: " + onR);
    }


    // hides icons once the player activates the cursor
    void HideIcons()
    {
        // if R is enabled, disables
        if (RCursor.enabled == true)
        {
            RCursor.enabled = false;
            onR = false;
        }
        
        // if Z is enabled, disables
        if (ZCursor.enabled == true)
        {
            ZCursor.enabled = false;
            onZ = false;
        }  
    }
}
