using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cursor : MonoBehaviour
{
    [SerializeField] float cursorSearchDistance = 0.8f;
    [SerializeField] Transform startingPosition = null;

    PlayerInput playerInput = null;
    MenuRotation menuRotation = null;

    bool cursorHidden = true;

    // caching
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        menuRotation = FindObjectOfType<MenuRotation>();
    }

    private void Start()
    {
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


    // applies vertical axis rounded to a positive or negative of the move distance
    void MoveVertical(float yInput)
    {
        if (cursorHidden == false)
        {
            // FOR USE LATER WITH RAYCAST
            // Vector3 startingPosition = transform.position;
            Vector3 translation = new Vector3(0, (yInput > 0 ? 1 : -1) * cursorSearchDistance * 100, 0);
            transform.Translate(translation);
        }
    }


    // applies horizontal axis rounded to a positive or negative of the move distance
    void MoveHorizontal(float xInput)
    {
        if(cursorHidden == false)
        {
            Vector3 translation = new Vector3((xInput > 0 ? 1 : -1) * cursorSearchDistance * 100, 0, 0);
            transform.Translate(translation);
        }
    }


    // sets cursor bool and gameobject to inactive
    void HideCursor()
    {
        cursorHidden = true;
    }


    // makes cursor visable and movable again
    void ShowCursor()
    {
        transform.position = startingPosition.position;
        gameObject.SetActive(true);
        cursorHidden = false;
    }
}
