using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Cursor : MonoBehaviour
{
    public event Action<string> SendName = delegate { };

    [SerializeField] float cursorSearchDistance = 0.8f;
    [SerializeField] Canvas mainUICanvas = null;
    [SerializeField] GameObject animationPrefab = null;

    // necessary scripts
    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;

    // necessary components
    GraphicRaycaster graphicRaycaster = null;
    PointerEventData pointerEventData = new PointerEventData(null);

    // persistent variables
    MenuItem currentItem = null;
    bool cursorHidden = true;

    // caching
    private void Awake()
    {
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        playerInput = FindObjectOfType<PlayerInput>();
        menuRotation = FindObjectOfType<MenuRotation>();
    }


    // sets cursor to hidden on start
    private void Start()
    {
        RaycastSearch(transform.position);
        cursorHidden = true;
    }


    #region event subscriptions
    // subscribing
    private void OnEnable()
    {
        playerInput.VerticalCursorMovement += MoveVertical;
        playerInput.HorizontalCursorMovement += MoveHorizontal;
        playerInput.CPress += EquipItem;
        menuRotation.AnimationStarted += HideCursor;
        menuRotation.AnimationFinished += ShowCursor;
    }

    private void OnDisable()
    {
        playerInput.VerticalCursorMovement -= MoveVertical;
        playerInput.HorizontalCursorMovement -= MoveHorizontal;
        playerInput.CPress -= EquipItem;
        menuRotation.AnimationStarted -= HideCursor;
        menuRotation.AnimationFinished -= ShowCursor;
    }
    #endregion


    // applies vertical axis to a positive or negative of the move distance
    void MoveVertical(float yInput)
    {
        if (cursorHidden == false)
        {
            // saves starting position, moves
            Vector3 startingPosition = transform.position;
            Vector3 translation = new Vector3(0, (yInput > 0 ? 1 : -1) * cursorSearchDistance, 0);
            transform.Translate(translation);

            // calls raycast search to read an object the cursor is over
            RaycastSearch(startingPosition);
        }
    }


    // applies horizontal axis rounded to a positive or negative of the move distance
    void MoveHorizontal(float xInput)
    {
        if(cursorHidden == false)
        {
            // saves starting position, moves
            Vector3 startingPosition = transform.position;
            Vector3 translation = new Vector3((xInput > 0 ? 1 : -1) * cursorSearchDistance, 0, 0);
            transform.Translate(translation);

            // calls raycast search to read an object the cursor is over
            RaycastSearch(startingPosition);
        }
    }


    // raycasts the canvas object the cursor is over to determine how the cursor moves
    void RaycastSearch(Vector3 position)
    {
        // moves pointer event data and fires a graphic raycast
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        //Debug.Log(results.Count);
        // if the raycast hits, saves current item, else returns cursor to starting position
        if (results.Count > 0)
        {
            if (results[0].gameObject.GetComponentInParent<MenuItem>() == true)
            {
                //Debug.Log(results[0].gameObject.name);
                currentItem = results[0].gameObject.GetComponentInParent<MenuItem>();

                //Debug.Log(currentItem.itemName);
                transform.position = new Vector3(results[0].gameObject.transform.position.x, results[0].gameObject.transform.position.y, transform.position.z);

            }
            else if (results[0].gameObject.GetComponentInParent<MenuItem>() == false)
            {
                //Debug.Log("branch 1");
                transform.position = position;
            }  
        }
        else
        {
            //Debug.Log("branch 2");
            transform.position = position;
        }       
    }


    // activates equip function in the currently selected item if the item is active
    void EquipItem(string cButton)
    {
        if(currentItem != null && currentItem.State == 1)
            currentItem.ItemAnimation(cButton);
    }


    // sets cursor bool and gameobject to inactive
    void HideCursor()
    {
        cursorHidden = true;
    }


    // makes cursor visable and movable again
    void ShowCursor(string screen)
    {
        if(screen == "item")
        {
            gameObject.SetActive(true);
            currentItem = null;
            cursorHidden = false;
        }
    }
}
