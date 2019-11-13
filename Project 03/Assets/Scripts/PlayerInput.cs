using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> Rotation = delegate { };
    public event Action StartPress = delegate { };
    public event Action<string> CPress = delegate { };
    public event Action<float> VerticalCursorMovement = delegate { };
    public event Action<float> HorizontalCursorMovement = delegate { };

    [SerializeField] float startButtonDelayTime = 0.6f;

    Coroutine startButtonCoroutine;

    // Update is called one per frame
    void Update()
    {
        DetectRotationInput();
        DetectStartInput();
        DetectCButtonInput();
        DetectCursorInput();
    }


    // calls left or right rotation with player input
    void DetectRotationInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            Rotation?.Invoke(Vector3.up);

        if (Input.GetKeyDown(KeyCode.E))
            Rotation?.Invoke(-Vector3.up);
    }


    // detects start press and begins delay coroutine
    void DetectStartInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) && startButtonCoroutine == null)
        {
            startButtonCoroutine = StartCoroutine(InputDelayRoutine());
            StartPress?.Invoke();
        }  
    }


    // sends action with current cursor input from WASD
    void DetectCursorInput()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            VerticalCursorMovement?.Invoke(Input.GetAxis("Vertical"));
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            HorizontalCursorMovement?.Invoke(Input.GetAxis("Horizontal"));
    }


    // assigns the correct C transform and sends it with an action
    void DetectCButtonInput()
    {
        string cButton = "";

        if (Input.GetKeyDown(KeyCode.J))
            cButton = "left";
        else if (Input.GetKeyDown(KeyCode.K))
            cButton = "down";
        else if (Input.GetKeyDown(KeyCode.L))
            cButton = "right";

        // checks if C current is null and sends action if not
        if (cButton != "")
        {
            CPress?.Invoke(cButton);
        }  
    }


    IEnumerator InputDelayRoutine()
    {
        yield return new WaitForSeconds(startButtonDelayTime);
        startButtonCoroutine = null;
    }
}
