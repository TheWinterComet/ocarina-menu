using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuUIAnimation : MonoBehaviour
{
    [SerializeField] Transform targetTransform = null;
    [SerializeField] float animationSpeed = 0.5f;

    // necessary components
    Image objectImage = null;
    MenuRotation menuRotation = null;

    // variables
    Vector3 startingPosition = Vector3.zero;
    Coroutine routine = null;


    // sets to false on awake
    private void Awake()
    {
        objectImage = GetComponent<Image>();
        menuRotation = FindObjectOfType<MenuRotation>();
    }


    #region subscriptions
    private void OnEnable()
    {
        menuRotation.OpenAnimationStarted += StartOpenAnimation;
        menuRotation.AnimationStarted += StartStandardAnimation;
        menuRotation.CloseAnimationStarted += HideObject;
    }

    private void OnDisable()
    {
        menuRotation.OpenAnimationStarted -= StartOpenAnimation;
        menuRotation.AnimationStarted -= StartStandardAnimation;
        menuRotation.CloseAnimationStarted += HideObject;
    }
    #endregion
    

    // establishes starting position
    private void Start()
    {
        objectImage.enabled = false;
        startingPosition = transform.position;
    }


    // starts open animation
    void StartOpenAnimation()
    {
        if (routine == null)
            routine = StartCoroutine(OpenAnimationRoutine());
    }


    // starts standard animation
    void StartStandardAnimation()
    {
        if(routine == null)
            routine = StartCoroutine(StandardAnimationRoutine());
    }


    // hides cursors on close
    void HideObject()
    {
        objectImage.enabled = false;
    }


    // on open, sets the object to active, and animates it moving inward
    IEnumerator OpenAnimationRoutine()
    {
        objectImage.enabled = true;
        transform.position = targetTransform.position;
        while(transform.position != startingPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, animationSpeed * Time.deltaTime * 700);
            yield return null;
        }
        routine = null;
        
    }


    // moves object first towards position and then away
    IEnumerator StandardAnimationRoutine()
    {
        while (transform.position != targetTransform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, animationSpeed * Time.deltaTime * 700);
            yield return null;
        }   

        while (transform.position != startingPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, animationSpeed * Time.deltaTime * 700);
            yield return null;
        }
            
        routine = null;
    }

}
