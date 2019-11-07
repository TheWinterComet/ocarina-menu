using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int currentHealth = 80;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        private set
        {
            currentHealth = value;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
