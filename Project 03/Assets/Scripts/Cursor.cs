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

    // necessary scripts
    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;

    // necessary components
    GraphicRaycaster graphicRaycaster = null;
    PointerEventData pointerEventData = new PointerEventData(null);

    // persistent variables
    Item currentItem = null;
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
        RaycastSearch(transform.position, Vector3.zero);
        cursorHidden = true;
    }


    #region event subscriptions
    // subscribing
    private void OnEnable()
    {
        playerInput.VerticalCursorMovement += MoveVertical;
        playerInput.HorizontalCursorMovement += MoveHorizontal;
        menuRotation.AnimationStarted += HideCursor;
        menuRotation.AnimationFinished += ShowCursor;
    }

    private void OnDisable()
    {
        playerInput.VerticalCursorMovement -= MoveVertical;
        playerInput.HorizontalCursorMovement -= MoveHorizontal;
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

            // calls raycast search to read an object the cursor is over
            RaycastSearch(startingPosition, translation);
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

            // calls raycast search to read an object the cursor is over
            RaycastSearch(startingPosition, translation);
        }
    }

    // TODO coordinate cursor behavior when moving over items not collected
    // raycasts the canvas object the cursor is over to determine how the cursor moves
    void RaycastSearch(Vector3 position, Vector3 translation)
    {
        transform.Translate(translation);

        // moves pointer event data and fires a graphic raycast
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        // if the raycast hits, saves current item, else returns cursor to starting position
        if (results.Count > 0)
        {
            if (results[0].gameObject.GetComponentInParent<MenuItem>() == true)
            {
                currentItem = results[0].gameObject.GetComponentInParent<MenuItem>().Item;
                transform.position = new Vector3(results[0].gameObject.transform.position.x, results[0].gameObject.transform.position.y, transform.position.z);

            }
            else if (results[0].gameObject.GetComponentInParent<MenuItem>() == false)
                transform.position = position;
        }
        else
            transform.position = position;
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
