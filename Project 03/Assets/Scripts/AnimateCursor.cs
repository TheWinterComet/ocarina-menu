using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCursor : MonoBehaviour
{
    [SerializeField] float flashTime = 1f, waitTime = 0.4f;

    CanvasRenderer canvasRenderer = null;
    Cursor cursor = null;

    Image animatedImage = null;
    Coroutine flashCoroutine = null;
    float timer = 0;

    // caching
    private void Awake()
    {
        animatedImage = GetComponent<Image>();
        canvasRenderer = GetComponent<CanvasRenderer>();
        cursor = GetComponentInParent<Cursor>();
        
    }


    #region subscriptions
    private void OnEnable()
    {
        cursor.CursorHidden += HideImage;
        cursor.CursorShown += ShowImage;
    }

    private void OnDisable()
    {
        cursor.CursorHidden -= HideImage;
        cursor.CursorShown -= ShowImage;
    }
    #endregion


    // hides image to start
    private void Start()
    {
        animatedImage.enabled = false;
    }


    // hides image
    void HideImage()
    {
        if(flashCoroutine != null)
            StopCoroutine(flashCoroutine);
        flashCoroutine = null;

        animatedImage.enabled = false;
        canvasRenderer.SetAlpha(0);
        timer = 0;
    }


    // shows image
    void ShowImage()
    {
        // reneables image and starts coroutine again
        animatedImage.enabled = true;
        if(flashCoroutine == null)
            flashCoroutine = StartCoroutine(FlashRoutine());
    }


    // flash animation for cursor object
    IEnumerator FlashRoutine()
    {
        // runs indefinitely
        while(true)
        {
            // waits
            yield return new WaitForSeconds(waitTime);

            // animates towards full opacity
            while(timer < 1)
            {
                timer += Time.deltaTime;
                canvasRenderer.SetAlpha(timer);

                if (timer >= 1)
                    timer = 1;

                yield return null;
            }

            // waits
            yield return new WaitForSeconds(waitTime);

            // animates towards full transparency
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                canvasRenderer.SetAlpha(timer);

                if (timer <= 0)
                    timer = 0;

                yield return null;
            }
        }
    }
}
