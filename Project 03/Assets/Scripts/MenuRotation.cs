using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MenuRotation : MonoBehaviour
{
    public event Action AnimationStarted = delegate { };
    public event Action AnimationFinished = delegate { };

    [Header("Rotation settings")]
    [SerializeField] float rotationTime = 1f;
    [SerializeField] float rotationDegrees = 90f;
    [SerializeField] List<GameObject> subscreens = new List<GameObject>();

    // scripts
    PlayerInput playerInput = null;

    // persistent variables
    Vector3 rotationDirection = Vector3.zero;
    float degreesPerSecond = 0f;
    float startPosition = 0f;
    float timer = 0;

    public bool MenuOpen { get; set; } = false;

    #region event subscriptions
    private void OnEnable()
    {
        playerInput.Rotation += StartRotation;
        playerInput.StartPress += MenuControl;
    }

    private void OnDisable()
    {
        playerInput.Rotation -= StartRotation;
        playerInput.StartPress -= MenuControl;
    }
    #endregion


    // caching
    private void Awake()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }


    // sets necessasry variables
    private void Start()
    {
        startPosition = transform.eulerAngles.y;
        degreesPerSecond = rotationDegrees / rotationTime;
    }


    // Update is called once per frame
    void Update()
    {
        RotateMenu();
    }


    // starts rotation sequence, called with direction from playerinput
    void StartRotation(Vector3 direction)
    {
        if (rotationDirection == Vector3.zero && MenuOpen == true)
        {
            // sends action to coordinate cursor
            AnimationStarted?.Invoke();

            rotationDirection = direction;
            timer = rotationTime;
        }  
    }

    // activates subscreens on open and starts their closing animation on close
    void MenuControl()
    {
        if (MenuOpen == false)
        {
            MenuOpen = true;

            // sets new starting position and rotates to the right on open
            startPosition = transform.eulerAngles.y - 90;
            transform.Rotate(0, -90, 0);
            StartRotation(Vector3.up);

            // sets each subscreen to active and enables scripts to play flip animation
            foreach (GameObject screen in subscreens)
            {
                screen.SetActive(true);
                SubscreenAnimation animationScript = screen.GetComponent<SubscreenAnimation>();
                animationScript.enabled = true;
            }
        }

        else if (MenuOpen == true)
        {
            // sends action for closing, but there's no close loop, so cursor should stay hidden until next open
            AnimationStarted?.Invoke();

            // activates close menu function in each subscreen
            foreach (GameObject screen in subscreens)
            {
                SubscreenAnimation animationScript = screen.GetComponent<SubscreenAnimation>();
                animationScript?.CloseMenuFlip();
            }
            MenuOpen = false;
        }
    }

    // rotates menu
    void RotateMenu()
    {
        if(timer > 0)
        {
            // rotates based on degrees per second and time since last frame and subtracts timer
            transform.Rotate(rotationDirection * degreesPerSecond * Time.deltaTime);
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                // resets timer
                timer = 0;

                // resets rotation values for next rotation
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, startPosition + (rotationDirection.y * rotationDegrees), transform.eulerAngles.z);
                startPosition = transform.eulerAngles.y;
                rotationDirection = Vector3.zero;

                // coordinates cursor
                AnimationFinished?.Invoke();
            }
        }
    }
}
