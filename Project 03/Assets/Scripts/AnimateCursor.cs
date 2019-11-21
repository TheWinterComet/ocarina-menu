using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateCursor : MonoBehaviour
{
    [SerializeField] float flashTime = 1f, waitTime = 0.4f;
    [SerializeField] CanvasRenderer canvasRenderer = null;
    float timer = 0;

    // caching
    private void Awake()
    {
        StartCoroutine(FlashRoutine());
    }

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
