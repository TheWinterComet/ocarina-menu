using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPulse : MonoBehaviour
{
    [SerializeField] float pulseTime = 0.6f;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HeartPulseRoutine());
    }

    IEnumerator HeartPulseRoutine()
    {
        while (true)
        {
            // animates towards large size
            while (timer < pulseTime)
            {
                timer += Time.deltaTime;
                transform.localScale = new Vector3(1 + timer / 2, 1 + timer / 2, transform.localScale.z);

                if (timer >= pulseTime)
                {
                    timer = pulseTime;
                    transform.localScale = new Vector3(1 + timer / 2, 1 + timer / 2, transform.localScale.z);
                }

                yield return null;
            }

            // animates towards regular size
            while (timer > 0)
            {
                timer -= Time.deltaTime;
                transform.localScale = new Vector3(1 + timer / 2, 1 + timer / 2, transform.localScale.z);

                if (timer <= 0)
                {
                    timer = 0;
                    transform.localScale = new Vector3(1, 1, transform.localScale.z);
                }

                yield return null;
            }
        }
    }
}
