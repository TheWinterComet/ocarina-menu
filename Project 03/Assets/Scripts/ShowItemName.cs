using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowItemName : MonoBehaviour
{
    [SerializeField] Cursor cursor = null;
    [SerializeField] MenuRotation menuRotation = null;
    Text nameText = null;


    // caching
    private void Awake()
    {
        nameText = GetComponent<Text>();
    }

    #region subscriptions
    private void OnEnable()
    {
        cursor.SendName += DisplayName;
    }

    private void OnDisable()
    {
        cursor.SendName -= DisplayName;
    }
    #endregion

    void DisplayName(string name = "")
    {
        nameText.text = name;
    }
}
