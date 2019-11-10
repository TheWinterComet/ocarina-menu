using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> Rotation = delegate { };
    public event Action StartPress = delegate { };
    public event Action<float> VerticalCursorMovement = delegate { };
    public event Action<float> HorizontalCursorMovement = delegate { };

    [SerializeField] float startButtonDelayTime = 0.6f;
    Coroutine startButtonCoroutine;

    // Update is called one per frame
    void Update()
    {
        DetectRotationInput();
        DetectStartInput();
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


    void DetectStartInput()
    {
        if (Input.GetKeyDown(KeyCode.Return) && startButtonCoroutine == null)
        {
            startButtonCoroutine = StartCoroutine(InputDelayRoutine());
            StartPress?.Invoke();
        }  
    }

    void DetectCursorInput()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
            VerticalCursorMovement?.Invoke(Input.GetAxis("Vertical"));
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
            HorizontalCursorMovement?.Invoke(Input.GetAxis("Horizontal"));
    }

    IEnumerator InputDelayRoutine()
    {
        yield return new WaitForSeconds(startButtonDelayTime);
        startButtonCoroutine = null;
    }
}
