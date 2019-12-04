using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Mask))]
public class SubscreenAnimation : MonoBehaviour
{
    [SerializeField] float rotationTime = 0.5f;
    [SerializeField] float rotationDegrees = 90f;

    float rotationDirection = 0;
    float opacityTimer = 0;

    Vector3 startLocalPosition;
    Quaternion startLocalRotation;
    RectTransform rectTransform = null;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }


    // Start is called before the first frame update
    void OnEnable()
    {
        // sets starting alpha to zero
        SetAlpha(0);

        // stores starting positions
        startLocalPosition = transform.localPosition;
        startLocalRotation = Quaternion.Euler(transform.localEulerAngles);

        // sets direction and starts timer
        rotationDirection = -1;
        opacityTimer = 0;
    }
    

    // calls flip subscreen
    private void Update()
    {
        FlipSubscreen();
    }


    // sets new direction and starts rotation;
    public void CloseMenuFlip()
    {
        rotationDirection = 1;
        opacityTimer = 0;
    }


    // flips subscreen based on a timer and the current flip direction
    void FlipSubscreen()
    {
        if (opacityTimer < rotationTime)
        {
            // determines rotation point, rotates
            Vector3 rotationPoint = rectTransform.position - rectTransform.up * rectTransform.sizeDelta.y / 2;
            transform.RotateAround(rotationPoint, rotationDirection * rectTransform.right, (rotationDegrees/rotationTime) * Time.deltaTime);


            // sets the current alpha based on the timer value
            opacityTimer += Time.deltaTime;
            SetAlpha(rotationDirection < 0 ? opacityTimer * (1 / rotationTime) : 1 - (opacityTimer * (1 / rotationTime)));

            if (opacityTimer >= rotationTime)
            {
                
                // resets timer
                opacityTimer = rotationTime;

                // sets final position based on the direction of the rotation
                if(rotationDirection < 0)
                {
                    transform.RotateAround(rotationPoint, rectTransform.right, 0 - transform.localEulerAngles.x);
                }
                if (rotationDirection > 0)
                {
                    // resets position
                    transform.localPosition = startLocalPosition;
                    transform.localEulerAngles = startLocalRotation.eulerAngles;

                    // disables object and script, sets menu open to false if not already
                    gameObject.SetActive(false);
                    enabled = false;
                }
            }
        }
    }

    // setalpha function
    void SetAlpha(float a)
    {
        // sets the alpha of the object's renderer
        CanvasRenderer canvasRenderer = GetComponent<CanvasRenderer>();
        canvasRenderer?.SetAlpha(a);

        // accesses children (if any) and sets the alpha of their renderers too
        if(transform.childCount > 0)
        {
            CanvasRenderer[] childCanvasRenderers = GetComponentsInChildren<CanvasRenderer>();
            foreach(CanvasRenderer renderer in childCanvasRenderers)
            {
                renderer?.SetAlpha(a);
            }
        }
    }
}
