using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIControl : MonoBehaviour
{
    [SerializeField] Image RCursor = null, ZCursor = null;

    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;
    Cursor cursor = null;
    bool onR = false, onZ = false;

    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        menuRotation = FindObjectOfType<MenuRotation>();
        cursor = FindObjectOfType<Cursor>();
    }

    private void OnEnable()
    {
        playerInput.HorizontalCursorMovement += ChangeMenu;
    }

    private void OnDisable()
    {
        playerInput.HorizontalCursorMovement -= ChangeMenu;
    }


    void ChangeMenu(float direction)
    {

    }


    public void ActivateRIcon()
    {
        RCursor.enabled = true;
        onR = true;
    }

    public void ActivateZIcon()
    {
        RCursor.enabled = true;
        onZ = true;
    }
}
