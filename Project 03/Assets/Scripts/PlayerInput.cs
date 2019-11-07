using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInput : MonoBehaviour
{
    public event Action<Vector3> Rotation = delegate { };
    public event Action StartPress = delegate { };

    [SerializeField] float startButtonDelayTime = 0.6f;
    Coroutine startButtonCoroutine;

    // Update is called one per frame
    void Update()
    {
        DetectRotationInput();
        DetectStartInput();
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

    IEnumerator InputDelayRoutine()
    {
        yield return new WaitForSeconds(startButtonDelayTime);
        startButtonCoroutine = null;
    }
}
