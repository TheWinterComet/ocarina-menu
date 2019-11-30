using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class Cursor : MonoBehaviour
{
    public event Action<string> SendName = delegate { };
    public event Action CursorHidden = delegate { };
    public event Action CursorShown = delegate { };

    [SerializeField] float cursorSearchDistance = 0.8f, cursorMovementDelayTime = 0.15f;
    [SerializeField] Canvas mainUICanvas = null;
    [SerializeField] GameObject animationPrefab = null;
    [SerializeField] AudioSource moveAudio = null, selectAudio = null;

    // necessary scripts
    PlayerInput playerInput = null;
    MenuUIControl menuUIControl = null;
    MenuRotation menuRotation = null;

    // necessary components
    Image mainCursorImage = null;
    GraphicRaycaster graphicRaycaster = null;
    PointerEventData pointerEventData = new PointerEventData(null);

    // persistent variables
    MenuItem currentItem = null;
    bool cursorHidden = true;

    Coroutine cursorCoroutine;

    // caching
    private void Awake()
    {
        mainCursorImage = GetComponent<Image>();
        graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        playerInput = FindObjectOfType<PlayerInput>();
        menuUIControl = FindObjectOfType<MenuUIControl>();
        menuRotation = FindObjectOfType<MenuRotation>();
    }


    // sets cursor to hidden on start
    private void Start()
    {
        RaycastSearch(transform.position);
        mainCursorImage.enabled = false;
    }


    #region event subscriptions
    // subscribing
    private void OnEnable()
    {
        playerInput.VerticalCursorMovement += MoveVertical;
        playerInput.HorizontalCursorMovement += MoveHorizontal;
        playerInput.CPress += EquipItem;
        menuRotation.AnimationStarted += HideCursor;

        HideCursor();
    }

    private void OnDisable()
    {
        playerInput.VerticalCursorMovement -= MoveVertical;
        playerInput.HorizontalCursorMovement -= MoveHorizontal;
        playerInput.CPress -= EquipItem;
        menuRotation.AnimationStarted -= HideCursor;
    }
    #endregion


    // applies vertical axis to a positive or negative of the move distance
    void MoveVertical(float yInput)
    {
        if (cursorHidden == false && cursorCoroutine == null)
        {
            cursorCoroutine = StartCoroutine(CursorDelayRoutine());

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
        if(cursorHidden == false && cursorCoroutine == null)
        {
            cursorCoroutine = StartCoroutine(CursorDelayRoutine());

            // saves starting position, moves
            Vector3 startingPosition = transform.position;
            Vector3 translation = new Vector3((xInput > 0 ? 1 : -1) * cursorSearchDistance, 0, 0);
            transform.Translate(translation);

            // calls raycast search to read an object the cursor is over
            bool foundItem = RaycastSearch(startingPosition);

            // if no item was found (meaning it moved horizontally off the item screen, moves it to the top slot in the far row and activates UI graphic
            if (foundItem == false)
            {
                // activates the r and z icons and sets the main cursor to inactive
                if(xInput > 0)
                    menuUIControl.ActivateRIcon();
                else if(xInput < 0)
                    menuUIControl.ActivateZIcon();

                // hides cursor
                moveAudio.Play();
                SendName?.Invoke("");
                HideCursor();
                CursorHidden?.Invoke();
            }
        }
    }


    // raycasts the canvas object the cursor is over to determine how the cursor moves
    bool RaycastSearch(Vector3 position)
    {
        // moves pointer event data and fires a graphic raycast
        pointerEventData.position = Camera.main.WorldToScreenPoint(transform.position);
        List<RaycastResult> results = new List<RaycastResult>();
        graphicRaycaster.Raycast(pointerEventData, results);

        // if the raycast hits, saves current item, else returns cursor to starting position
        if (results.Count > 0)
        {
            if (results[0].gameObject.GetComponentInParent<MenuItem>() == true)
            {
                // stores reference to the item function
                currentItem = results[0].gameObject.GetComponentInParent<MenuItem>();

                // transforms cursor directly to the center of the found object, plays respective feedback
                transform.position = new Vector3
                    (results[0].gameObject.transform.position.x, results[0].gameObject.transform.position.y, transform.position.z);
                moveAudio.Play();
                SendName?.Invoke(currentItem.itemName);
                return true;

            }
            else if (results[0].gameObject.GetComponentInParent<MenuItem>() == false)
            {
                // returns cursor to previous transform position (effectively, it doesn't move)
                transform.position = position;
                return false;
            }
        }
        else
        {
            // also reverts cursor position to check errors
            transform.position = position;
            return false;
        }
        return false;
    }


    // activates equip function in the currently selected item if the item is active
    void EquipItem(string cButton)
    {
        if(currentItem != null && cursorHidden == false && currentItem.State == 1)
        {
            selectAudio.Play();
            currentItem.ItemAnimation(cButton);
        }
            
    }


    // sets cursor bool and gameobject to inactive
    void HideCursor()
    {
        SendName?.Invoke("");
        cursorHidden = true;
        CursorHidden?.Invoke();
        mainCursorImage.enabled = false;
    }


    // makes cursor visable and movable again
    public void ShowCursor(float direction)
    {
        Vector3 startingPosition = transform.position;

        // moves the cursor to the highest spot vertically
        do
        {
            startingPosition = transform.position;
            transform.Translate(new Vector3(0, 1 * cursorSearchDistance, 0));
        }
        while (RaycastSearch(startingPosition));

        // moves the cursor to the last horizontal spot closest to the icon being moved off
        do
        {
            startingPosition = transform.position;
            transform.Translate(new Vector3((direction > 0 ? -1 : 1) * cursorSearchDistance, 0, 0));
        }
        while (RaycastSearch(startingPosition));

        // enables cursor
        mainCursorImage.enabled = true;
        RaycastSearch(transform.position);
        cursorHidden = false;
        CursorShown?.Invoke();

        // starts cursor coroutine to prevent it from moving immediately
        cursorCoroutine = StartCoroutine(CursorDelayRoutine());  
    }


    IEnumerator CursorDelayRoutine()
    {
        yield return new WaitForSeconds(cursorMovementDelayTime);
        cursorCoroutine = null;
    }
}
