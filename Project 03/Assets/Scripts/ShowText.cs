using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowText : MonoBehaviour
{
    [SerializeField] Cursor cursor = null;
    [SerializeField] Text nameText = null, infoText = null;
    MenuUIControl menuUIControl = null;

    private void Awake()
    {
        menuUIControl = GetComponentInParent<MenuUIControl>();
    }

    #region subscriptions
    private void OnEnable()
    {
        cursor.SendName += DisplayName;
        menuUIControl.SendInfo += DisplayInfo;
    }

    private void OnDisable()
    {
        cursor.SendName -= DisplayName;
        menuUIControl.SendInfo -= DisplayInfo;
    }
    #endregion

    void DisplayName(string name = "")
    {
        nameText.text = name;
    }

    void DisplayInfo(string info = "")
    {
        infoText.text = info;
    }
}
